using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace NGCP.IO
{
    public abstract class Link
    {
        /// <summary>
        /// Amount of byte limit max of a single package include escape tokens
        /// </summary>
        public int PackageMaxSizeLimit = 65536;
        /// <summary>
        /// User defined token of ending a package
        /// </summary>
        public byte[] EscapeToken = { (byte)'\n' };
        /// <summary>
        /// User defined function to find the end of package 
        /// </summary>
        public Func<byte[], int> FindPackageEnd = (b) => b.Length;
        /// <summary>
        /// Action to do when a new package was received
        /// </summary>
        public Action<byte[]> PackageReceived;
        /// <summary>
        /// Start a Link
        /// </summary>
        public abstract void Start();
        /// <summary>
        /// Stop a Link
        /// </summary>
        public abstract void Stop();
        /// <summary>
        /// Send data
        /// </summary>
        /// <param name="bytes"></param>
        public virtual void Send(byte[] bytes) { }
    }
}


