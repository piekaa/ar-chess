using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine.SocialPlatforms;

public enum StateResultAction
{
    NEW_OBJECT,
    NEW_ARRAY,
    END_OBJECT,
    END_ARRAY,
    NEW_OBJECT_IN_ARRAY,
    NONE
}

public enum WhereAmI
{
    OBJECT,
    ARRAY,
    NOWHERE,
}

public class StateResult
{
    public IJsonState NextState;
    public string Key;
    public string Value;
    public string ArrayValue;
    public StateResultAction stateResultAction = StateResultAction.NONE;

    public StateResult(IJsonState nextState)
    {
        NextState = nextState;
    }

    public StateResult(IJsonState nextState, string key, string value, string arrayValue,
        StateResultAction stateResultAction) : this(nextState)
    {
        Key = key;
        Value = value;
        ArrayValue = arrayValue;
        this.stateResultAction = stateResultAction;
    }
}

public abstract class IJsonState
{
    public abstract StateResult HandleChar(char c);

    public virtual void whereAmI(WhereAmI whereAmI)
    {
    }

    public virtual bool InString()
    {
        return false;
    }
}

public class BeginJsonState : IJsonState
{
    public override StateResult HandleChar(char c)
    {
        if (c == '{')
        {
            return new StateResult(new ObjectJsonState(), null, null, null, StateResultAction.NEW_OBJECT);
        }
        else if (c == '[')
        {
            return new StateResult(new ArrayJsonState(), null, null, null, StateResultAction.NEW_ARRAY);
        }

        throw new InvalidOperationException("expected { at begining");
    }
}

public abstract class BaseValueJsonState : IJsonState
{
    private bool first = true;
    private string t = "true";
    private string f = "false";
    private bool boolean;
    private bool isNull;
    private StringBuilder sb = new StringBuilder();

    protected abstract IJsonState nextOpenState();
    protected abstract StateResultAction openObjectAction();

    protected abstract char endChar();

    protected abstract StateResult stateResultAfterComma(string value);

    protected abstract StateResult endState(string value);

    public override StateResult HandleChar(char c)
    {
        if (first)
        {
            first = false;
            if (c >= '0' && c <= '9' || c == '-')
            {
                sb.Append(c);
                return null;
            }

            if (c == 't' || c == 'f')
            {
                sb.Append(c);
                boolean = true;
                return null;
            }

            if (c == 'n')
            {
                sb.Append(c);
                isNull = true;
                return null;
            }

            if (c == '\"')
            {
                return new StateResult(nextOpenState(), null, null, null, StateResultAction.NONE);
            }

            if (c == '{')
            {
                return new StateResult(new ObjectJsonState(), null, null, null, openObjectAction());
            }

            if (c == '[')
            {
                return new StateResult(new ArrayJsonState(), null, null, null, StateResultAction.NEW_ARRAY);
            }

            if (c == endChar())
            {
                return endState(sb.ToString());
            }

            throw new InvalidOperationException("expected numeric value or \"");
        }

        if ((c >= '0' && c <= '9') || c == '.' || t.Contains(c.ToString()) || f.Contains(c.ToString()) ||
            "null".Contains(c))
        {
            sb.Append(c);
            if (boolean && !t.StartsWith(sb.ToString()) && !f.StartsWith(sb.ToString()))
            {
                throw new InvalidOperationException("Expected true or false, but was: " + sb);
            }

            if (isNull && !"null".StartsWith(sb.ToString()))
            {
                throw new InvalidOperationException("Expected null, but was: " + sb);
            }

            return null;
        }

        if (c == ',')
        {
            return stateResultAfterComma(sb.ToString());
        }

        if (c == '}')
        {
            return new StateResult(new EndObjectOrArrayState(), null, sb.ToString(), null,
                StateResultAction.END_OBJECT);
        }

        if (c == endChar())
        {
            return endState(sb.ToString());
        }

        {
            throw new InvalidOperationException("expected numeric value or , or " + endChar());
        }
    }
}


public class ArrayJsonState : BaseValueJsonState
{
    protected override IJsonState nextOpenState()
    {
        return new OpenedArrayValueJsonState();
    }

    protected override StateResultAction openObjectAction()
    {
        return StateResultAction.NEW_OBJECT_IN_ARRAY;
    }

    protected override char endChar()
    {
        return ']';
    }

    protected override StateResult stateResultAfterComma(string value)
    {
        return new StateResult(new ArrayJsonState(), null, null, value, StateResultAction.NONE);
    }

    protected override StateResult endState(string value)
    {
        return new StateResult(new EndObjectOrArrayState(), null, null, value, StateResultAction.END_ARRAY);
    }
}

public class ValueJsonState : BaseValueJsonState
{
    protected override IJsonState nextOpenState()
    {
        return new OpenedValueJsonState();
    }

    protected override StateResultAction openObjectAction()
    {
        return StateResultAction.NEW_OBJECT;
    }

    protected override char endChar()
    {
        return '}';
    }

    protected override StateResult stateResultAfterComma(string value)
    {
        return new StateResult(new ObjectJsonState(), null, value, null, StateResultAction.NONE);
    }

    protected override StateResult endState(string value)
    {
        return new StateResult(new EndObjectOrArrayState(), null, value, null, StateResultAction.END_OBJECT);
    }
}

public class ObjectJsonState : IJsonState
{
    public override StateResult HandleChar(char c)
    {
        if (c != '\"')
        {
            throw new InvalidOperationException("expected \"");
        }

        return new StateResult(new OpenedKeyJsonState());
    }
}

public class OpenedKeyJsonState : IJsonState
{
    private StringBuilder sb = new StringBuilder();

    public override StateResult HandleChar(char c)
    {
        if (c == '\"')
        {
            return new StateResult(new ClosedKeyJsonState(), sb.ToString(), null, null, StateResultAction.NONE);
        }

        sb.Append(c);
        return null;
    }
}

public class ClosedKeyJsonState : IJsonState
{
    public override StateResult HandleChar(char c)
    {
        if (c != ':')
        {
            throw new InvalidOperationException("expected :");
        }

        return new StateResult(new ValueJsonState());
    }
}

public class OpenedValueJsonState : IJsonState
{
    private StringBuilder sb = new StringBuilder();
    private bool escape = false;

    public override StateResult HandleChar(char c)
    {
        if (c == '\"' && !escape)
        {
            return new StateResult(new ClosedValueJsonState(), null, sb.ToString(), null, StateResultAction.NONE);
        }

        if (c == '\\' && !escape)
        {
            escape = true;
        }
        else
        {
            escape = false;
            sb.Append(c);
        }

        return null;
    }

    public override bool InString()
    {
        return true;
    }
}

public class OpenedArrayValueJsonState : IJsonState
{
    private StringBuilder sb = new StringBuilder();
    private bool escape = false;

    public override StateResult HandleChar(char c)
    {
        if (c == '\"' && !escape)
        {
            return new StateResult(new ClosedArrayValueJsonState(), null, null, sb.ToString(), StateResultAction.NONE);
        }

        if (c == '\\' && !escape)
        {
            escape = true;
        }
        else
        {
            escape = false;
            sb.Append(c);
        }

        return null;
    }

    public override bool InString()
    {
        return true;
    }
}

public class ClosedArrayValueJsonState : IJsonState
{
    public override StateResult HandleChar(char c)
    {
        if (c == ',')
        {
            return new StateResult(new ArrayJsonState());
        }
        else if (c == ']')
        {
            return new StateResult(new EndObjectOrArrayState(), null, null, null, StateResultAction.END_ARRAY);
        }
        else
        {
            throw new InvalidOperationException("expected , or ]");
        }
    }
}

public class ClosedValueJsonState : IJsonState
{
    public override StateResult HandleChar(char c)
    {
        if (c == ',')
        {
            return new StateResult(new ObjectJsonState());
        }
        else if (c == '}')
        {
            return new StateResult(new EndObjectOrArrayState(), null, null, null, StateResultAction.END_OBJECT);
        }
        else
        {
            throw new InvalidOperationException("expected , or }");
        }
    }
}

public class EndObjectOrArrayState : IJsonState
{
    private WhereAmI where;

    public override void whereAmI(WhereAmI whereAmI)
    {
        where = whereAmI;
    }

    public override StateResult HandleChar(char c)
    {
        if (c == ',')
        {
            if (where == WhereAmI.OBJECT)
            {
                return new StateResult(new ObjectJsonState());
            }
            else
            {
                return new StateResult(new ArrayJsonState());
            }
        }

        if (c == '}')
        {
            return new StateResult(new EndObjectOrArrayState(), null, null, null, StateResultAction.END_OBJECT);
        }

        if (c == ']')
        {
            return new StateResult(new EndObjectOrArrayState(), null, null, null, StateResultAction.END_ARRAY);
        }

        throw new InvalidOperationException("expected , or }, or ]");
    }
}