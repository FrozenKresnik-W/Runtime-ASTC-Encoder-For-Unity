using System;
using System.Runtime.InteropServices;

namespace Aperture.Astcenc.Runtime
{
    public class AstcRaw : IDisposable
    {
        internal IntPtr Data
        {
            get
            {
                return m_Data;
            }
        }
        private IntPtr m_Data;

        internal int Length
        {
            get
            {
                return m_Length;
            }
        }
        private int m_Length;

        private bool m_Disposed;

        public AstcRaw(IntPtr data, int length)
        {
            m_Data = data;
            m_Length = length;
            m_Disposed = false;
        }

        ~AstcRaw()
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
                m_Length = 0;

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
