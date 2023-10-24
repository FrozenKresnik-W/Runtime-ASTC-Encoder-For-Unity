using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Aperture.Astcenc.Runtime
{
    public interface IRawData
    {
        void WriteTo(IntPtr destPtr);
    }

    public struct R8 : IRawData
    {
        public float RFloat
        {
            get
            {
                return m_R / 255.0f;
            }
        }
        private byte m_R;

        public void WriteTo(IntPtr destPtr)
        {
            Marshal.WriteByte(destPtr + 0, m_R);
            Marshal.WriteByte(destPtr + 1, 0);
            Marshal.WriteByte(destPtr + 2, 0);
            Marshal.WriteByte(destPtr + 3, 0xFF);
        }
    }

    public struct RG8 : IRawData
    {
        public float RFloat
        {
            get
            {
                return m_R / 255.0f;
            }
        }
        private byte m_R;

        public float GFloat
        {
            get
            {
                return m_G / 255.0f;
            }
        }
        private byte m_G;

        public void WriteTo(IntPtr destPtr)
        {
            Marshal.WriteByte(destPtr + 0, m_R);
            Marshal.WriteByte(destPtr + 1, m_G);
            Marshal.WriteByte(destPtr + 2, 0);
            Marshal.WriteByte(destPtr + 3, 0xFF);
        }
    }

    public struct RGB8 : IRawData
    {
        public float RFloat
        {
            get
            {
                return m_R / 255.0f;
            }
        }
        private byte m_R;

        public float GFloat
        {
            get
            {
                return m_G / 255.0f;
            }
        }
        private byte m_G;

        public float BFloat
        {
            get
            {
                return m_B / 255.0f;
            }
        }
        private byte m_B;

        public void WriteTo(IntPtr destPtr)
        {
            Marshal.WriteByte(destPtr + 0, m_R);
            Marshal.WriteByte(destPtr + 1, m_G);
            Marshal.WriteByte(destPtr + 2, m_B);
            Marshal.WriteByte(destPtr + 3, 0xFF);
        }
    }

    public struct RGBA8 : IRawData
    {
        public float RFloat
        {
            get
            {
                return m_R / 255.0f;
            }
        }
        private byte m_R;

        public float GFloat
        {
            get
            {
                return m_G / 255.0f;
            }
        }
        private byte m_G;

        public float BFloat
        {
            get
            {
                return m_B / 255.0f;
            }
        }
        private byte m_B;

        public float AFloat
        {
            get
            {
                return m_A / 255.0f;
            }
        }
        private byte m_A;

        public void WriteTo(IntPtr destPtr)
        {
            Marshal.WriteByte(destPtr + 0, m_R);
            Marshal.WriteByte(destPtr + 1, m_G);
            Marshal.WriteByte(destPtr + 2, m_B);
            Marshal.WriteByte(destPtr + 3, m_A);
        }
    }

    public struct R16 : IRawData
    {
        internal ushort RHalf
        {
            get
            {
                return Mathf.FloatToHalf(RFloat);
            }
        }

        public float RFloat
        {
            get
            {
                return m_R / 65535.0f;
            }
        }
        private ushort m_R;

        public void WriteTo(IntPtr destPtr)
        {
            byte[] r = BitConverter.GetBytes(RHalf);
            Marshal.WriteByte(destPtr + 0, r[0]);
            Marshal.WriteByte(destPtr + 1, r[1]);

            Marshal.WriteByte(destPtr + 2, 0);
            Marshal.WriteByte(destPtr + 3, 0);

            Marshal.WriteByte(destPtr + 4, 0);
            Marshal.WriteByte(destPtr + 5, 0);

            Marshal.WriteByte(destPtr + 6, 0x00);
            Marshal.WriteByte(destPtr + 7, 0x3C);
        }
    }

    public struct RG16 : IRawData
    {
        internal ushort RHalf
        {
            get
            {
                return Mathf.FloatToHalf(RFloat);
            }
        }

        public float RFloat
        {
            get
            {
                return m_R / 65535.0f;
            }
        }
        private ushort m_R;

        internal ushort GHalf
        {
            get
            {
                return Mathf.FloatToHalf(GFloat);
            }
        }

        public float GFloat
        {
            get
            {
                return m_G / 65535.0f;
            }
        }
        private ushort m_G;

        public void WriteTo(IntPtr destPtr)
        {
            byte[] r = BitConverter.GetBytes(RHalf);
            Marshal.WriteByte(destPtr + 0, r[0]);
            Marshal.WriteByte(destPtr + 1, r[1]);

            byte[] g = BitConverter.GetBytes(GHalf);
            Marshal.WriteByte(destPtr + 2, g[0]);
            Marshal.WriteByte(destPtr + 3, g[1]);

            Marshal.WriteByte(destPtr + 4, 0);
            Marshal.WriteByte(destPtr + 5, 0);

            Marshal.WriteByte(destPtr + 6, 0x00);
            Marshal.WriteByte(destPtr + 7, 0x3C);
        }
    }

    public struct RGB16 : IRawData
    {
        internal ushort RHalf
        {
            get
            {
                return Mathf.FloatToHalf(RFloat);
            }
        }

        public float RFloat
        {
            get
            {
                return m_R / 65535.0f;
            }
        }
        private ushort m_R;

        internal ushort GHalf
        {
            get
            {
                return Mathf.FloatToHalf(GFloat);
            }
        }

        public float GFloat
        {
            get
            {
                return m_G / 65535.0f;
            }
        }
        private ushort m_G;

        internal ushort BHalf
        {
            get
            {
                return Mathf.FloatToHalf(BFloat);
            }
        }

        public float BFloat
        {
            get
            {
                return m_B / 65535.0f;
            }
        }
        private ushort m_B;

        public void WriteTo(IntPtr destPtr)
        {
            byte[] r = BitConverter.GetBytes(RHalf);
            Marshal.WriteByte(destPtr + 0, r[0]);
            Marshal.WriteByte(destPtr + 1, r[1]);

            byte[] g = BitConverter.GetBytes(GHalf);
            Marshal.WriteByte(destPtr + 2, g[0]);
            Marshal.WriteByte(destPtr + 3, g[1]);

            byte[] b = BitConverter.GetBytes(BHalf);
            Marshal.WriteByte(destPtr + 4, b[0]);
            Marshal.WriteByte(destPtr + 5, b[1]);

            Marshal.WriteByte(destPtr + 6, 0x00);
            Marshal.WriteByte(destPtr + 7, 0x3C);
        }
    }

    public struct RGBA16 : IRawData
    {
        internal ushort RHalf
        {
            get
            {
                return Mathf.FloatToHalf(RFloat);
            }
        }

        public float RFloat
        {
            get
            {
                return m_R / 65535.0f;
            }
        }
        private ushort m_R;

        internal ushort GHalf
        {
            get
            {
                return Mathf.FloatToHalf(GFloat);
            }
        }

        public float GFloat
        {
            get
            {
                return m_G / 65535.0f;
            }
        }
        private ushort m_G;

        internal ushort BHalf
        {
            get
            {
                return Mathf.FloatToHalf(BFloat);
            }
        }

        public float BFloat
        {
            get
            {
                return m_B / 65535.0f;
            }
        }
        private ushort m_B;

        internal ushort AHalf
        {
            get
            {
                return Mathf.FloatToHalf(AFloat);
            }
        }

        public float AFloat
        {
            get
            {
                return m_A / 65535.0f;
            }
        }
        private ushort m_A;

        public void WriteTo(IntPtr destPtr)
        {
            byte[] r = BitConverter.GetBytes(RHalf);
            Marshal.WriteByte(destPtr + 0, r[0]);
            Marshal.WriteByte(destPtr + 1, r[1]);

            byte[] g = BitConverter.GetBytes(GHalf);
            Marshal.WriteByte(destPtr + 2, g[0]);
            Marshal.WriteByte(destPtr + 3, g[1]);

            byte[] b = BitConverter.GetBytes(BHalf);
            Marshal.WriteByte(destPtr + 4, b[0]);
            Marshal.WriteByte(destPtr + 5, b[1]);

            byte[] a = BitConverter.GetBytes(AHalf);
            Marshal.WriteByte(destPtr + 6, a[0]);
            Marshal.WriteByte(destPtr + 7, a[1]);
        }
    }

    public struct R16F : IRawData
    {
        public float RFloat
        {
            get
            {
                return Mathf.HalfToFloat(m_R);
            }
        }
        private ushort m_R;

        public void WriteTo(IntPtr destPtr)
        {
            byte[] r = BitConverter.GetBytes(m_R);
            Marshal.WriteByte(destPtr + 0, r[0]);
            Marshal.WriteByte(destPtr + 1, r[1]);

            Marshal.WriteByte(destPtr + 2, 0);
            Marshal.WriteByte(destPtr + 3, 0);

            Marshal.WriteByte(destPtr + 4, 0);
            Marshal.WriteByte(destPtr + 5, 0);

            Marshal.WriteByte(destPtr + 6, 0x00);
            Marshal.WriteByte(destPtr + 7, 0x3C);
        }
    }

    public struct RG16F : IRawData
    {
        public float RFloat
        {
            get
            {
                return Mathf.HalfToFloat(m_R);
            }
        }
        private ushort m_R;

        public float GFloat
        {
            get
            {
                return Mathf.HalfToFloat(m_G);
            }
        }
        private ushort m_G;

        public void WriteTo(IntPtr destPtr)
        {
            byte[] r = BitConverter.GetBytes(m_R);
            Marshal.WriteByte(destPtr + 0, r[0]);
            Marshal.WriteByte(destPtr + 1, r[1]);

            byte[] g = BitConverter.GetBytes(m_G);
            Marshal.WriteByte(destPtr + 2, g[0]);
            Marshal.WriteByte(destPtr + 3, g[1]);

            Marshal.WriteByte(destPtr + 4, 0);
            Marshal.WriteByte(destPtr + 5, 0);

            Marshal.WriteByte(destPtr + 6, 0x00);
            Marshal.WriteByte(destPtr + 7, 0x3C);
        }
    }

    public struct RGB16F : IRawData
    {
        public float RFloat
        {
            get
            {
                return Mathf.HalfToFloat(m_R);
            }
        }
        private ushort m_R;

        public float GFloat
        {
            get
            {
                return Mathf.HalfToFloat(m_G);
            }
        }
        private ushort m_G;

        public float BFloat
        {
            get
            {
                return Mathf.HalfToFloat(m_B);
            }
        }
        public ushort m_B;

        public void WriteTo(IntPtr destPtr)
        {
            byte[] r = BitConverter.GetBytes(m_R);
            Marshal.WriteByte(destPtr + 0, r[0]);
            Marshal.WriteByte(destPtr + 1, r[1]);

            byte[] g = BitConverter.GetBytes(m_G);
            Marshal.WriteByte(destPtr + 2, g[0]);
            Marshal.WriteByte(destPtr + 3, g[1]);

            byte[] b = BitConverter.GetBytes(m_B);
            Marshal.WriteByte(destPtr + 4, b[0]);
            Marshal.WriteByte(destPtr + 5, b[1]);

            Marshal.WriteByte(destPtr + 6, 0x00);
            Marshal.WriteByte(destPtr + 7, 0x3C);
        }
    }

    public struct RGBA16F : IRawData
    {
        public float RFloat
        {
            get
            {
                return Mathf.HalfToFloat(m_R);
            }
        }
        private ushort m_R;

        public float GFloat
        {
            get
            {
                return Mathf.HalfToFloat(m_G);
            }
        }
        private ushort m_G;

        public float BFloat
        {
            get
            {
                return Mathf.HalfToFloat(m_B);
            }
        }
        private ushort m_B;

        public float AFloat
        {
            get
            {
                return Mathf.HalfToFloat(m_A);
            }
        }
        private ushort m_A;

        public void WriteTo(IntPtr destPtr)
        {
            byte[] r = BitConverter.GetBytes(m_R);
            Marshal.WriteByte(destPtr + 0, r[0]);
            Marshal.WriteByte(destPtr + 1, r[1]);

            byte[] g = BitConverter.GetBytes(m_G);
            Marshal.WriteByte(destPtr + 2, g[0]);
            Marshal.WriteByte(destPtr + 3, g[1]);

            byte[] b = BitConverter.GetBytes(m_B);
            Marshal.WriteByte(destPtr + 4, b[0]);
            Marshal.WriteByte(destPtr + 5, b[1]);

            byte[] a = BitConverter.GetBytes(m_A);
            Marshal.WriteByte(destPtr + 6, a[0]);
            Marshal.WriteByte(destPtr + 7, a[1]);
        }
    }

    public struct R32F : IRawData
    {
        public float RFloat
        {
            get
            {
                return BitConverter.ToSingle(BitConverter.GetBytes(m_R));
            }
        }
        private uint m_R;

        public void WriteTo(IntPtr destPtr)
        {
            byte[] r = BitConverter.GetBytes(m_R);
            Marshal.WriteByte(destPtr + 0, r[0]);
            Marshal.WriteByte(destPtr + 1, r[1]);
            Marshal.WriteByte(destPtr + 2, r[2]);
            Marshal.WriteByte(destPtr + 3, r[3]);

            Marshal.WriteByte(destPtr + 4,  0);
            Marshal.WriteByte(destPtr + 5,  0);
            Marshal.WriteByte(destPtr + 6,  0);
            Marshal.WriteByte(destPtr + 7,  0);

            Marshal.WriteByte(destPtr + 8,  0);
            Marshal.WriteByte(destPtr + 9,  0);
            Marshal.WriteByte(destPtr + 10, 0);
            Marshal.WriteByte(destPtr + 11, 0);

            Marshal.WriteByte(destPtr + 12, 0);
            Marshal.WriteByte(destPtr + 13, 0);
            Marshal.WriteByte(destPtr + 14, 0x80);
            Marshal.WriteByte(destPtr + 15, 0x3F);

        }
    }

    public struct RG32F : IRawData
    {
        public float RFloat
        {
            get
            {
                return BitConverter.ToSingle(BitConverter.GetBytes(m_R));
            }
        }
        private uint m_R;

        public float GFloat
        {
            get
            {
                return BitConverter.ToSingle(BitConverter.GetBytes(m_G));
            }
        }
        private uint m_G;

        public void WriteTo(IntPtr destPtr)
        {
            byte[] r = BitConverter.GetBytes(m_R);
            Marshal.WriteByte(destPtr + 0, r[0]);
            Marshal.WriteByte(destPtr + 1, r[1]);
            Marshal.WriteByte(destPtr + 2, r[2]);
            Marshal.WriteByte(destPtr + 3, r[3]);

            byte[] g = BitConverter.GetBytes(m_G);
            Marshal.WriteByte(destPtr + 4, g[0]);
            Marshal.WriteByte(destPtr + 5, g[1]);
            Marshal.WriteByte(destPtr + 6, g[2]);
            Marshal.WriteByte(destPtr + 7, g[3]);

            Marshal.WriteByte(destPtr + 8,  0);
            Marshal.WriteByte(destPtr + 9,  0);
            Marshal.WriteByte(destPtr + 10, 0);
            Marshal.WriteByte(destPtr + 11, 0);

            Marshal.WriteByte(destPtr + 12, 0);
            Marshal.WriteByte(destPtr + 13, 0);
            Marshal.WriteByte(destPtr + 14, 0x80);
            Marshal.WriteByte(destPtr + 15, 0x3F);
        }
    }

    public struct RGB32F : IRawData
    {
        public float RFloat
        {
            get
            {
                return BitConverter.ToSingle(BitConverter.GetBytes(m_R));
            }
        }
        private uint m_R;

        public float GFloat
        {
            get
            {
                return BitConverter.ToSingle(BitConverter.GetBytes(m_G));
            }
        }
        private uint m_G;

        public float BFloat
        {
            get
            {
                return BitConverter.ToSingle(BitConverter.GetBytes(m_B));
            }
        }
        private uint m_B;

        public void WriteTo(IntPtr destPtr)
        {
            byte[] r = BitConverter.GetBytes(m_R);
            Marshal.WriteByte(destPtr + 0, r[0]);
            Marshal.WriteByte(destPtr + 1, r[1]);
            Marshal.WriteByte(destPtr + 2, r[2]);
            Marshal.WriteByte(destPtr + 3, r[3]);

            byte[] g = BitConverter.GetBytes(m_G);
            Marshal.WriteByte(destPtr + 4, g[0]);
            Marshal.WriteByte(destPtr + 5, g[1]);
            Marshal.WriteByte(destPtr + 6, g[2]);
            Marshal.WriteByte(destPtr + 7, g[3]);

            byte[] b = BitConverter.GetBytes(m_B);
            Marshal.WriteByte(destPtr + 8,  b[0]);
            Marshal.WriteByte(destPtr + 9,  b[1]);
            Marshal.WriteByte(destPtr + 10, b[2]);
            Marshal.WriteByte(destPtr + 11, b[3]);

            Marshal.WriteByte(destPtr + 12, 0);
            Marshal.WriteByte(destPtr + 13, 0);
            Marshal.WriteByte(destPtr + 14, 0x80);
            Marshal.WriteByte(destPtr + 15, 0x3F);
        }
    }

    public struct RGBA32F : IRawData
    {
        public float RFloat
        {
            get
            {
                return BitConverter.ToSingle(BitConverter.GetBytes(m_R));
            }
        }
        private uint m_R;

        public float GFloat
        {
            get
            {
                return BitConverter.ToSingle(BitConverter.GetBytes(m_G));
            }
        }
        private uint m_G;

        public float BFloat
        {
            get
            {
                return BitConverter.ToSingle(BitConverter.GetBytes(m_B));
            }
        }
        private uint m_B;

        public float AFloat
        {
            get
            {
                return BitConverter.ToSingle(BitConverter.GetBytes(m_A));
            }
        }
        private uint m_A;

        public void WriteTo(IntPtr destPtr)
        {
            byte[] r = BitConverter.GetBytes(m_R);
            Marshal.WriteByte(destPtr + 0, r[0]);
            Marshal.WriteByte(destPtr + 1, r[1]);
            Marshal.WriteByte(destPtr + 2, r[2]);
            Marshal.WriteByte(destPtr + 3, r[3]);

            byte[] g = BitConverter.GetBytes(m_G);
            Marshal.WriteByte(destPtr + 4, g[0]);
            Marshal.WriteByte(destPtr + 5, g[1]);
            Marshal.WriteByte(destPtr + 6, g[2]);
            Marshal.WriteByte(destPtr + 7, g[3]);

            byte[] b = BitConverter.GetBytes(m_B);
            Marshal.WriteByte(destPtr + 8,  b[0]);
            Marshal.WriteByte(destPtr + 9,  b[1]);
            Marshal.WriteByte(destPtr + 10, b[2]);
            Marshal.WriteByte(destPtr + 11, b[3]);

            byte[] a = BitConverter.GetBytes(m_A);
            Marshal.WriteByte(destPtr + 12, a[0]);
            Marshal.WriteByte(destPtr + 13, a[1]);
            Marshal.WriteByte(destPtr + 14, a[2]);
            Marshal.WriteByte(destPtr + 15, a[3]);

        }
    }
}
