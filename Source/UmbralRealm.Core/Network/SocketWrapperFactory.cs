﻿using System.Net.Sockets;
using UmbralRealm.Core.Utilities;

namespace UmbralRealm.Core.Network
{
    public class SocketWrapperFactory
    {
        public SocketWrapper Create()
        {
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            return new SocketWrapper(socket);
        }
    }
}