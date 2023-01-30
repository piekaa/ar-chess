﻿using UnityEngine;

public class LCDChar : MonoBehaviour
{
    [SerializeField] private DigitMaterials digitMaterials;
    [SerializeField] private MeshRenderer[] pixels;

    public char ToDisplay = 'a';


    private void Update()
    {
        int currentPixel = pixels.Length - 1;

        if (ToDisplay > 255)
        {
            ToDisplay = (char)0;
        }

        foreach (var line in font[ToDisplay])
        {
            var testBit = 1 << 7;
            for (int i = 0; i < 8; i++)
            {
                if ((line & testBit) > 0)
                {
                    pixels[currentPixel].material = digitMaterials.On;
                }
                else
                {
                    pixels[currentPixel].material = digitMaterials.Off;
                }

                testBit >>= 1;
                currentPixel--;
            }
        }
    }

    private int[][] font = new int[256][]
    {
        new[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, // 0x00
        new[] { 0x00, 0x3E, 0x45, 0x51, 0x45, 0x3E }, // 0x01
        new[] { 0x00, 0x3E, 0x6B, 0x6F, 0x6B, 0x3E }, // 0x02
        new[] { 0x00, 0x1C, 0x3E, 0x7C, 0x3E, 0x1C }, // 0x03
        new[] { 0x00, 0x18, 0x3C, 0x7E, 0x3C, 0x18 }, // 0x04
        new[] { 0x00, 0x30, 0x36, 0x7F, 0x36, 0x30 }, // 0x05
        new[] { 0x00, 0x18, 0x5C, 0x7E, 0x5C, 0x18 }, // 0x06
        new[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, // 0x07
        new[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, // 0x08
        new[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, // 0x09
        new[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, // 0x0A
        new[] { 0x00, 0x30, 0x48, 0x4A, 0x36, 0x0E }, // 0x0B
        new[] { 0x00, 0x06, 0x29, 0x79, 0x29, 0x06 }, // 0x0C
        new[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, // 0x0D
        new[] { 0x00, 0x60, 0x7E, 0x0A, 0x35, 0x3F }, // 0x0E
        new[] { 0x00, 0x2A, 0x1C, 0x36, 0x1C, 0x2A }, // 0x0F
        new[] { 0x00, 0x00, 0x7F, 0x3E, 0x1C, 0x08 }, // 0x10
        new[] { 0x00, 0x08, 0x1C, 0x3E, 0x7F, 0x00 }, // 0x11
        new[] { 0x00, 0x14, 0x36, 0x7F, 0x36, 0x14 }, // 0x12
        new[] { 0x00, 0x00, 0x5F, 0x00, 0x5F, 0x00 }, // 0x13
        new[] { 0x00, 0x06, 0x09, 0x7F, 0x01, 0x7F }, // 0x14
        new[] { 0x00, 0x22, 0x4D, 0x55, 0x59, 0x22 }, // 0x15
        new[] { 0x00, 0x60, 0x60, 0x60, 0x60, 0x00 }, // 0x16
        new[] { 0x00, 0x14, 0xB6, 0xFF, 0xB6, 0x14 }, // 0x17
        new[] { 0x00, 0x04, 0x06, 0x7F, 0x06, 0x04 }, // 0x18
        new[] { 0x00, 0x10, 0x30, 0x7F, 0x30, 0x10 }, // 0x19
        new[] { 0x00, 0x08, 0x08, 0x3E, 0x1C, 0x08 }, // 0x1A
        new[] { 0x00, 0x08, 0x1C, 0x3E, 0x08, 0x08 }, // 0x1B
        new[] { 0x00, 0x78, 0x40, 0x40, 0x40, 0x40 }, // 0x1C
        new[] { 0x00, 0x08, 0x3E, 0x08, 0x3E, 0x08 }, // 0x1D
        new[] { 0x00, 0x30, 0x3C, 0x3F, 0x3C, 0x30 }, // 0x1E
        new[] { 0x00, 0x03, 0x0F, 0x3F, 0x0F, 0x03 }, // 0x1F
        new[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, // 0x20
        new[] { 0x00, 0x00, 0x06, 0x5F, 0x06, 0x00 }, // 0x21
        new[] { 0x00, 0x07, 0x03, 0x00, 0x07, 0x03 }, // 0x22
        new[] { 0x00, 0x24, 0x7E, 0x24, 0x7E, 0x24 }, // 0x23
        new[] { 0x00, 0x24, 0x2B, 0x6A, 0x12, 0x00 }, // 0x24
        new[] { 0x00, 0x63, 0x13, 0x08, 0x64, 0x63 }, // 0x25
        new[] { 0x00, 0x36, 0x49, 0x56, 0x20, 0x50 }, // 0x26
        new[] { 0x00, 0x00, 0x07, 0x03, 0x00, 0x00 }, // 0x27
        new[] { 0x00, 0x00, 0x3E, 0x41, 0x00, 0x00 }, // 0x28
        new[] { 0x00, 0x00, 0x41, 0x3E, 0x00, 0x00 }, // 0x29
        new[] { 0x00, 0x08, 0x3E, 0x1C, 0x3E, 0x08 }, // 0x2A
        new[] { 0x00, 0x08, 0x08, 0x3E, 0x08, 0x08 }, // 0x2B
        new[] { 0x00, 0x00, 0xE0, 0x60, 0x00, 0x00 }, // 0x2C
        new[] { 0x00, 0x08, 0x08, 0x08, 0x08, 0x08 }, // 0x2D
        new[] { 0x00, 0x00, 0x60, 0x60, 0x00, 0x00 }, // 0x2E
        new[] { 0x00, 0x20, 0x10, 0x08, 0x04, 0x02 }, // 0x2F
        new[] { 0x00, 0x3E, 0x51, 0x49, 0x45, 0x3E }, // 0x30
        new[] { 0x00, 0x00, 0x42, 0x7F, 0x40, 0x00 }, // 0x31
        new[] { 0x00, 0x62, 0x51, 0x49, 0x49, 0x46 }, // 0x32
        new[] { 0x00, 0x22, 0x49, 0x49, 0x49, 0x36 }, // 0x33
        new[] { 0x00, 0x18, 0x14, 0x12, 0x7F, 0x10 }, // 0x34
        new[] { 0x00, 0x2F, 0x49, 0x49, 0x49, 0x31 }, // 0x35
        new[] { 0x00, 0x3C, 0x4A, 0x49, 0x49, 0x30 }, // 0x36
        new[] { 0x00, 0x01, 0x71, 0x09, 0x05, 0x03 }, // 0x37
        new[] { 0x00, 0x36, 0x49, 0x49, 0x49, 0x36 }, // 0x38
        new[] { 0x00, 0x06, 0x49, 0x49, 0x29, 0x1E }, // 0x39
        new[] { 0x00, 0x00, 0x6C, 0x6C, 0x00, 0x00 }, // 0x3A
        new[] { 0x00, 0x00, 0xEC, 0x6C, 0x00, 0x00 }, // 0x3B
        new[] { 0x00, 0x08, 0x14, 0x22, 0x41, 0x00 }, // 0x3C
        new[] { 0x00, 0x24, 0x24, 0x24, 0x24, 0x24 }, // 0x3D
        new[] { 0x00, 0x00, 0x41, 0x22, 0x14, 0x08 }, // 0x3E
        new[] { 0x00, 0x02, 0x01, 0x59, 0x09, 0x06 }, // 0x3F
        new[] { 0x00, 0x3E, 0x41, 0x5D, 0x55, 0x1E }, // 0x40
        new[] { 0x00, 0x7E, 0x11, 0x11, 0x11, 0x7E }, // 0x41
        new[] { 0x00, 0x7F, 0x49, 0x49, 0x49, 0x36 }, // 0x42
        new[] { 0x00, 0x3E, 0x41, 0x41, 0x41, 0x22 }, // 0x43
        new[] { 0x00, 0x7F, 0x41, 0x41, 0x41, 0x3E }, // 0x44
        new[] { 0x00, 0x7F, 0x49, 0x49, 0x49, 0x41 }, // 0x45
        new[] { 0x00, 0x7F, 0x09, 0x09, 0x09, 0x01 }, // 0x46
        new[] { 0x00, 0x3E, 0x41, 0x49, 0x49, 0x7A }, // 0x47
        new[] { 0x00, 0x7F, 0x08, 0x08, 0x08, 0x7F }, // 0x48
        new[] { 0x00, 0x00, 0x41, 0x7F, 0x41, 0x00 }, // 0x49
        new[] { 0x00, 0x30, 0x40, 0x40, 0x40, 0x3F }, // 0x4A
        new[] { 0x00, 0x7F, 0x08, 0x14, 0x22, 0x41 }, // 0x4B
        new[] { 0x00, 0x7F, 0x40, 0x40, 0x40, 0x40 }, // 0x4C
        new[] { 0x00, 0x7F, 0x02, 0x04, 0x02, 0x7F }, // 0x4D
        new[] { 0x00, 0x7F, 0x02, 0x04, 0x08, 0x7F }, // 0x4E
        new[] { 0x00, 0x3E, 0x41, 0x41, 0x41, 0x3E }, // 0x4F
        new[] { 0x00, 0x7F, 0x09, 0x09, 0x09, 0x06 }, // 0x50
        new[] { 0x00, 0x3E, 0x41, 0x51, 0x21, 0x5E }, // 0x51
        new[] { 0x00, 0x7F, 0x09, 0x09, 0x19, 0x66 }, // 0x52
        new[] { 0x00, 0x26, 0x49, 0x49, 0x49, 0x32 }, // 0x53
        new[] { 0x00, 0x01, 0x01, 0x7F, 0x01, 0x01 }, // 0x54
        new[] { 0x00, 0x3F, 0x40, 0x40, 0x40, 0x3F }, // 0x55
        new[] { 0x00, 0x1F, 0x20, 0x40, 0x20, 0x1F }, // 0x56
        new[] { 0x00, 0x3F, 0x40, 0x3C, 0x40, 0x3F }, // 0x57
        new[] { 0x00, 0x63, 0x14, 0x08, 0x14, 0x63 }, // 0x58
        new[] { 0x00, 0x07, 0x08, 0x70, 0x08, 0x07 }, // 0x59
        new[] { 0x00, 0x71, 0x49, 0x45, 0x43, 0x00 }, // 0x5A
        new[] { 0x00, 0x00, 0x7F, 0x41, 0x41, 0x00 }, // 0x5B
        new[] { 0x00, 0x02, 0x04, 0x08, 0x10, 0x20 }, // 0x5C
        new[] { 0x00, 0x00, 0x41, 0x41, 0x7F, 0x00 }, // 0x5D
        new[] { 0x00, 0x04, 0x02, 0x01, 0x02, 0x04 }, // 0x5E
        new[] { 0x80, 0x80, 0x80, 0x80, 0x80, 0x80 }, // 0x5F
        new[] { 0x00, 0x00, 0x03, 0x07, 0x00, 0x00 }, // 0x60
        new[] { 0x00, 0x20, 0x54, 0x54, 0x54, 0x78 }, // 0x61
        new[] { 0x00, 0x7F, 0x44, 0x44, 0x44, 0x38 }, // 0x62
        new[] { 0x00, 0x38, 0x44, 0x44, 0x44, 0x28 }, // 0x63
        new[] { 0x00, 0x38, 0x44, 0x44, 0x44, 0x7F }, // 0x64
        new[] { 0x00, 0x38, 0x54, 0x54, 0x54, 0x08 }, // 0x65
        new[] { 0x00, 0x08, 0x7E, 0x09, 0x09, 0x00 }, // 0x66
        new[] { 0x00, 0x18, 0xA4, 0xA4, 0xA4, 0x7C }, // 0x67
        new[] { 0x00, 0x7F, 0x04, 0x04, 0x78, 0x00 }, // 0x68
        new[] { 0x00, 0x00, 0x00, 0x7D, 0x40, 0x00 }, // 0x69
        new[] { 0x00, 0x40, 0x80, 0x84, 0x7D, 0x00 }, // 0x6A
        new[] { 0x00, 0x7F, 0x10, 0x28, 0x44, 0x00 }, // 0x6B
        new[] { 0x00, 0x00, 0x00, 0x7F, 0x40, 0x00 }, // 0x6C
        new[] { 0x00, 0x7C, 0x04, 0x18, 0x04, 0x78 }, // 0x6D
        new[] { 0x00, 0x7C, 0x04, 0x04, 0x78, 0x00 }, // 0x6E
        new[] { 0x00, 0x38, 0x44, 0x44, 0x44, 0x38 }, // 0x6F
        new[] { 0x00, 0xFC, 0x44, 0x44, 0x44, 0x38 }, // 0x70
        new[] { 0x00, 0x38, 0x44, 0x44, 0x44, 0xFC }, // 0x71
        new[] { 0x00, 0x44, 0x78, 0x44, 0x04, 0x08 }, // 0x72
        new[] { 0x00, 0x08, 0x54, 0x54, 0x54, 0x20 }, // 0x73
        new[] { 0x00, 0x04, 0x3E, 0x44, 0x24, 0x00 }, // 0x74
        new[] { 0x00, 0x3C, 0x40, 0x20, 0x7C, 0x00 }, // 0x75
        new[] { 0x00, 0x1C, 0x20, 0x40, 0x20, 0x1C }, // 0x76
        new[] { 0x00, 0x3C, 0x60, 0x30, 0x60, 0x3C }, // 0x77
        new[] { 0x00, 0x6C, 0x10, 0x10, 0x6C, 0x00 }, // 0x78
        new[] { 0x00, 0x9C, 0xA0, 0x60, 0x3C, 0x00 }, // 0x79
        new[] { 0x00, 0x64, 0x54, 0x54, 0x4C, 0x00 }, // 0x7A
        new[] { 0x00, 0x08, 0x3E, 0x41, 0x41, 0x00 }, // 0x7B
        new[] { 0x00, 0x00, 0x00, 0x77, 0x00, 0x00 }, // 0x7C
        new[] { 0x00, 0x00, 0x41, 0x41, 0x3E, 0x08 }, // 0x7D
        new[] { 0x00, 0x02, 0x01, 0x02, 0x01, 0x00 }, // 0x7E
        new[] { 0x00, 0x3C, 0x26, 0x23, 0x26, 0x3C }, // 0x7F
        new[] { 0x00, 0x1E, 0xA1, 0xE1, 0x21, 0x12 }, // 0x80
        new[] { 0x00, 0x3D, 0x40, 0x20, 0x7D, 0x00 }, // 0x81
        new[] { 0x00, 0x38, 0x54, 0x54, 0x55, 0x09 }, // 0x82
        new[] { 0x00, 0x20, 0x55, 0x55, 0x55, 0x78 }, // 0x83
        new[] { 0x00, 0x20, 0x55, 0x54, 0x55, 0x78 }, // 0x84
        new[] { 0x00, 0x20, 0x55, 0x55, 0x54, 0x78 }, // 0x85
        new[] { 0x00, 0x20, 0x57, 0x55, 0x57, 0x78 }, // 0x86
        new[] { 0x00, 0x1C, 0xA2, 0xE2, 0x22, 0x14 }, // 0x87
        new[] { 0x00, 0x38, 0x55, 0x55, 0x55, 0x08 }, // 0x88
        new[] { 0x00, 0x38, 0x55, 0x54, 0x55, 0x08 }, // 0x89
        new[] { 0x00, 0x38, 0x55, 0x55, 0x54, 0x08 }, // 0x8A
        new[] { 0x00, 0x00, 0x01, 0x7C, 0x41, 0x00 }, // 0x8B
        new[] { 0x00, 0x00, 0x01, 0x7D, 0x41, 0x00 }, // 0x8C
        new[] { 0x00, 0x00, 0x01, 0x7C, 0x40, 0x00 }, // 0x8D
        new[] { 0x00, 0x70, 0x29, 0x24, 0x29, 0x70 }, // 0x8E
        new[] { 0x00, 0x78, 0x2F, 0x25, 0x2F, 0x78 }, // 0x8F
        new[] { 0x00, 0x7C, 0x54, 0x54, 0x55, 0x45 }, // 0x90
        new[] { 0x00, 0x34, 0x54, 0x7C, 0x54, 0x58 }, // 0x91
        new[] { 0x00, 0x7E, 0x09, 0x7F, 0x49, 0x49 }, // 0x92
        new[] { 0x00, 0x38, 0x45, 0x45, 0x39, 0x00 }, // 0x93
        new[] { 0x00, 0x38, 0x45, 0x44, 0x39, 0x00 }, // 0x94
        new[] { 0x00, 0x39, 0x45, 0x44, 0x38, 0x00 }, // 0x95
        new[] { 0x00, 0x3C, 0x41, 0x21, 0x7D, 0x00 }, // 0x96
        new[] { 0x00, 0x3D, 0x41, 0x20, 0x7C, 0x00 }, // 0x97
        new[] { 0x00, 0x9C, 0xA1, 0x60, 0x3D, 0x00 }, // 0x98
        new[] { 0x00, 0x3D, 0x42, 0x42, 0x3D, 0x00 }, // 0x99
        new[] { 0x00, 0x3C, 0x41, 0x40, 0x3D, 0x00 }, // 0x9A
        new[] { 0x80, 0x70, 0x68, 0x58, 0x38, 0x04 }, // 0x9B
        new[] { 0x00, 0x48, 0x3E, 0x49, 0x49, 0x62 }, // 0x9C
        new[] { 0x00, 0x7E, 0x61, 0x5D, 0x43, 0x3F }, // 0x9D
        new[] { 0x00, 0x22, 0x14, 0x08, 0x14, 0x22 }, // 0x9E
        new[] { 0x00, 0x40, 0x88, 0x7E, 0x09, 0x02 }, // 0x9F
        new[] { 0x00, 0x20, 0x54, 0x55, 0x55, 0x78 }, // 0xA0
        new[] { 0x00, 0x00, 0x00, 0x7D, 0x41, 0x00 }, // 0xA1
        new[] { 0x00, 0x38, 0x44, 0x45, 0x39, 0x00 }, // 0xA2
        new[] { 0x00, 0x3C, 0x40, 0x21, 0x7D, 0x00 }, // 0xA3
        new[] { 0x00, 0x7A, 0x09, 0x0A, 0x71, 0x00 }, // 0xA4
        new[] { 0x00, 0x7A, 0x11, 0x22, 0x79, 0x00 }, // 0xA5
        new[] { 0x00, 0x08, 0x55, 0x55, 0x55, 0x5E }, // 0xA6
        new[] { 0x00, 0x4E, 0x51, 0x51, 0x4E, 0x00 }, // 0xA7
        new[] { 0x00, 0x30, 0x48, 0x4D, 0x40, 0x20 }, // 0xA8
        new[] { 0x3E, 0x41, 0x5D, 0x4B, 0x55, 0x3E }, // 0xA9
        new[] { 0x04, 0x04, 0x04, 0x04, 0x04, 0x1C }, // 0xAA
        new[] { 0x00, 0x17, 0x08, 0x4C, 0x6A, 0x50 }, // 0xAB
        new[] { 0x00, 0x17, 0x08, 0x34, 0x2A, 0x78 }, // 0xAC
        new[] { 0x00, 0x00, 0x30, 0x7D, 0x30, 0x00 }, // 0xAD
        new[] { 0x00, 0x08, 0x14, 0x00, 0x08, 0x14 }, // 0xAE
        new[] { 0x00, 0x14, 0x08, 0x00, 0x14, 0x08 }, // 0xAF
        new[] { 0x44, 0x11, 0x44, 0x11, 0x44, 0x11 }, // 0xB0
        new[] { 0xAA, 0x55, 0xAA, 0x55, 0xAA, 0x55 }, // 0xB1
        new[] { 0xBB, 0xEE, 0xBB, 0xEE, 0xBB, 0xEE }, // 0xB2
        new[] { 0x00, 0x00, 0x00, 0xFF, 0x00, 0x00 }, // 0xB3
        new[] { 0x08, 0x08, 0x08, 0xFF, 0x00, 0x00 }, // 0xB4
        new[] { 0x00, 0x70, 0x28, 0x25, 0x29, 0x70 }, // 0xB5
        new[] { 0x00, 0x70, 0x29, 0x25, 0x29, 0x70 }, // 0xB6
        new[] { 0x00, 0x70, 0x29, 0x25, 0x28, 0x70 }, // 0xB7
        new[] { 0x3E, 0x41, 0x5D, 0x55, 0x41, 0x3E }, // 0xB8
        new[] { 0x0A, 0xFB, 0x00, 0xFF, 0x00, 0x00 }, // 0xB9
        new[] { 0x00, 0xFF, 0x00, 0xFF, 0x00, 0x00 }, // 0xBA
        new[] { 0x0A, 0xFA, 0x02, 0xFE, 0x00, 0x00 }, // 0xBB
        new[] { 0x0A, 0x0B, 0x08, 0x0F, 0x00, 0x00 }, // 0xBC
        new[] { 0x00, 0x18, 0x24, 0x66, 0x24, 0x00 }, // 0xBD
        new[] { 0x00, 0x29, 0x2A, 0x7C, 0x2A, 0x29 }, // 0xBE
        new[] { 0x08, 0x08, 0x08, 0xF8, 0x00, 0x00 }, // 0xBF
        new[] { 0x00, 0x00, 0x00, 0x0F, 0x08, 0x08 }, // 0xC0
        new[] { 0x08, 0x08, 0x08, 0x0F, 0x08, 0x08 }, // 0xC1
        new[] { 0x08, 0x08, 0x08, 0xF8, 0x08, 0x08 }, // 0xC2
        new[] { 0x00, 0x00, 0x00, 0xFF, 0x08, 0x08 }, // 0xC3
        new[] { 0x08, 0x08, 0x08, 0x08, 0x08, 0x08 }, // 0xC4
        new[] { 0x08, 0x08, 0x08, 0xFF, 0x08, 0x08 }, // 0xC5
        new[] { 0x00, 0x20, 0x56, 0x55, 0x56, 0x79 }, // 0xC6
        new[] { 0x00, 0x70, 0x2A, 0x25, 0x2A, 0x71 }, // 0xC7
        new[] { 0x00, 0x0F, 0x08, 0x0B, 0x0A, 0x0A }, // 0xC8
        new[] { 0x00, 0xFE, 0x02, 0xFA, 0x0A, 0x0A }, // 0xC9
        new[] { 0x0A, 0x0B, 0x08, 0x0B, 0x0A, 0x0A }, // 0xCA
        new[] { 0x0A, 0xFA, 0x02, 0xFA, 0x0A, 0x0A }, // 0xCB
        new[] { 0x00, 0xFF, 0x00, 0xFB, 0x0A, 0x0A }, // 0xCC
        new[] { 0x0A, 0x0A, 0x0A, 0x0A, 0x0A, 0x0A }, // 0xCD
        new[] { 0x0A, 0xFB, 0x00, 0xFB, 0x0A, 0x0A }, // 0xCE
        new[] { 0x00, 0x5D, 0x22, 0x22, 0x22, 0x5D }, // 0xCF
        new[] { 0x00, 0x22, 0x55, 0x59, 0x30, 0x00 }, // 0xD0
        new[] { 0x00, 0x08, 0x7F, 0x49, 0x41, 0x3E }, // 0xD1
        new[] { 0x00, 0x7C, 0x55, 0x55, 0x55, 0x44 }, // 0xD2
        new[] { 0x00, 0x7C, 0x55, 0x54, 0x55, 0x44 }, // 0xD3
        new[] { 0x00, 0x7C, 0x55, 0x55, 0x54, 0x44 }, // 0xD4
        new[] { 0x00, 0x00, 0x00, 0x07, 0x00, 0x00 }, // 0xD5
        new[] { 0x00, 0x00, 0x44, 0x7D, 0x45, 0x00 }, // 0xD6
        new[] { 0x00, 0x00, 0x45, 0x7D, 0x45, 0x00 }, // 0xD7
        new[] { 0x00, 0x00, 0x45, 0x7C, 0x45, 0x00 }, // 0xD8
        new[] { 0x08, 0x08, 0x08, 0x0F, 0x00, 0x00 }, // 0xD9
        new[] { 0x00, 0x00, 0x00, 0xF8, 0x08, 0x08 }, // 0xDA
        new[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }, // 0xDB
        new[] { 0xF0, 0xF0, 0xF0, 0xF0, 0xF0, 0xF0 }, // 0xDC
        new[] { 0x00, 0x00, 0x00, 0x77, 0x00, 0x00 }, // 0xDD
        new[] { 0x00, 0x00, 0x45, 0x7D, 0x44, 0x00 }, // 0xDE
        new[] { 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F }, // 0xDF
        new[] { 0x00, 0x3C, 0x42, 0x43, 0x3D, 0x00 }, // 0xE0
        new[] { 0x00, 0xFE, 0x4A, 0x4A, 0x34, 0x00 }, // 0xE1
        new[] { 0x00, 0x3C, 0x43, 0x43, 0x3D, 0x00 }, // 0xE2
        new[] { 0x00, 0x3D, 0x43, 0x42, 0x3C, 0x00 }, // 0xE3
        new[] { 0x00, 0x32, 0x49, 0x4A, 0x31, 0x00 }, // 0xE4
        new[] { 0x00, 0x3A, 0x45, 0x46, 0x39, 0x00 }, // 0xE5
        new[] { 0x00, 0xFC, 0x20, 0x20, 0x1C, 0x00 }, // 0xE6
        new[] { 0x00, 0xFE, 0xAA, 0x28, 0x10, 0x00 }, // 0xE7
        new[] { 0x00, 0xFF, 0xA5, 0x24, 0x18, 0x00 }, // 0xE8
        new[] { 0x00, 0x3C, 0x40, 0x41, 0x3D, 0x00 }, // 0xE9
        new[] { 0x00, 0x3C, 0x41, 0x41, 0x3D, 0x00 }, // 0xEA
        new[] { 0x00, 0x3D, 0x41, 0x40, 0x3C, 0x00 }, // 0xEB
        new[] { 0x00, 0x9C, 0xA0, 0x61, 0x3D, 0x00 }, // 0xEC
        new[] { 0x00, 0x04, 0x08, 0x71, 0x09, 0x04 }, // 0xED
        new[] { 0x00, 0x00, 0x02, 0x02, 0x02, 0x00 }, // 0xEE
        new[] { 0x00, 0x00, 0x07, 0x03, 0x00, 0x00 }, // 0xEF
        new[] { 0x00, 0x00, 0x08, 0x08, 0x08, 0x00 }, // 0xF0
        new[] { 0x00, 0x00, 0x24, 0x2E, 0x24, 0x00 }, // 0xF1
        new[] { 0x00, 0x24, 0x24, 0x24, 0x24, 0x24 }, // 0xF2
        new[] { 0x05, 0x17, 0x0A, 0x34, 0x2A, 0x78 }, // 0xF3
        new[] { 0x00, 0x06, 0x09, 0x7F, 0x01, 0x7F }, // 0xF4
        new[] { 0x00, 0x22, 0x4D, 0x55, 0x59, 0x22 }, // 0xF5
        new[] { 0x00, 0x08, 0x08, 0x2A, 0x08, 0x08 }, // 0xF6
        new[] { 0x00, 0x00, 0x08, 0x18, 0x18, 0x00 }, // 0xF7
        new[] { 0x00, 0x06, 0x09, 0x09, 0x06, 0x00 }, // 0xF8
        new[] { 0x00, 0x00, 0x08, 0x00, 0x08, 0x00 }, // 0xF9
        new[] { 0x00, 0x00, 0x08, 0x00, 0x00, 0x00 }, // 0xFA
        new[] { 0x00, 0x02, 0x0F, 0x00, 0x00, 0x00 }, // 0xFB
        new[] { 0x00, 0x09, 0x0F, 0x05, 0x00, 0x00 }, // 0xFC
        new[] { 0x00, 0x09, 0x0D, 0x0A, 0x00, 0x00 }, // 0xFD
        new[] { 0x00, 0x3C, 0x3C, 0x3C, 0x3C, 0x00 }, // 0xFE
        new[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } // 0xFF
    };
}