using System;
using System.Runtime.InteropServices;

namespace Aperture.Astcenc.Runtime
{
    public class AstcHeader : IDisposable
    {
        private const uint MAGIC_FILE_CONSTANT = 0x5CA1AB13;

        internal IntPtr Data
        {
            get
            {
                return m_Data;
            }
        }
        private IntPtr m_Data;

        private bool m_Disposed;

        public AstcHeader(byte[] data)
        {
            m_Data = Marshal.AllocHGlobal(16);

            for(int i = 0; i < 16; i++)
            {
                Marshal.WriteByte(m_Data + i, data[i]);
            }
            m_Disposed = false;
        }

        public AstcHeader(uint dimX, uint dimY, uint dimZ, int sizeX, int sizeY, int sizeZ)
        {
            m_Data = Marshal.AllocHGlobal(16);

            Marshal.WriteByte(m_Data + 0, (byte)(MAGIC_FILE_CONSTANT & 0xFF));
            Marshal.WriteByte(m_Data + 1, (byte)((MAGIC_FILE_CONSTANT >> 8) & 0xFF));
            Marshal.WriteByte(m_Data + 2, (byte)((MAGIC_FILE_CONSTANT >> 16) & 0xFF));
            Marshal.WriteByte(m_Data + 3, (byte)((MAGIC_FILE_CONSTANT >> 24) & 0xFF));

            Marshal.WriteByte(m_Data + 4, (byte)dimX);
            Marshal.WriteByte(m_Data + 5, (byte)dimY);
            Marshal.WriteByte(m_Data + 6, (byte)dimZ);

            Marshal.WriteByte(m_Data + 7, (byte)(sizeX & 0xFF));
            Marshal.WriteByte(m_Data + 8, (byte)((sizeX >> 8) & 0xFF));
            Marshal.WriteByte(m_Data + 9, (byte)((sizeX >> 16) & 0xFF));

            Marshal.WriteByte(m_Data + 10, (byte)(sizeY & 0xFF));
            Marshal.WriteByte(m_Data + 11, (byte)((sizeY >> 8) & 0xFF));
            Marshal.WriteByte(m_Data + 12, (byte)((sizeY >> 16) & 0xFF));

            Marshal.WriteByte(m_Data + 13, (byte)(sizeZ & 0xFF));
            Marshal.WriteByte(m_Data + 14, (byte)((sizeZ >> 8) & 0xFF));
            Marshal.WriteByte(m_Data + 15, (byte)((sizeZ >> 16) & 0xFF));
        }

        ~AstcHeader()
        {
            Dispose(false);
        }

        public void ThrowIfDisposed()
        {
            if (m_Disposed)
                throw new ObjectDisposedException(GetType().FullName);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!m_Disposed)
            {
                if (disposing)
                {

                }

                if (m_Data != IntPtr.Zero)
                    Marshal.FreeHGlobal(m_Data);

                m_Data = IntPtr.Zero;
                m_Disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
