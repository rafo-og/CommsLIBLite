﻿using CommsLIBLite.Helper;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace CommsLIBLite.Communications
{
    public static class TimeOutSocketFactory
    {
        #region logger
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        #endregion

        public static TcpClient Connect(IPEndPoint remoteEndPoint, int timeoutMSec)
        {
            TcpClient tcpclient = new TcpClient();
            int startTime = TimeTools.GetCoarseMillisNow();

            try
            {
                IAsyncResult asyncResult = tcpclient.BeginConnect(remoteEndPoint.Address, remoteEndPoint.Port, null, null);

                if (asyncResult.AsyncWaitHandle.WaitOne(timeoutMSec, false))
                {
                    try
                    {
                        tcpclient.EndConnect(asyncResult);
                        return tcpclient;
                    }
                    catch
                    {
                        // See if we have to wait a little bit
                        int wait = timeoutMSec - (TimeTools.GetCoarseMillisNow() - startTime);
                        if (wait > 10)
                            Thread.Sleep(wait);

                        tcpclient?.Dispose();
                        return null;
                    }
                }
                else
                {
                    // See if we have to wait a little bit
                    int wait = timeoutMSec - (TimeTools.GetCoarseMillisNow() - startTime);
                    if (wait > 10)
                        Thread.Sleep(wait);
                    try
                    {
                        tcpclient?.Dispose();
                        return null;
                    }
                    catch { }

                    return null;
                    
                }
            } catch (Exception e)
            {
                logger.Error(e, $"Error in TimeOutSocketFactory {remoteEndPoint.Address.ToString()} - {remoteEndPoint.Port.ToString()}" );
                // See if we have to wait a little bit
                int wait = timeoutMSec - (TimeTools.GetCoarseMillisNow() - startTime);
                if (wait > 10)
                    Thread.Sleep(wait);
                try
                {
                    tcpclient?.Dispose();
                }
                catch { }

                return null;
            }
            
        }
    }
}
