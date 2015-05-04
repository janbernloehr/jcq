//    Copyright 2008 Jan Molnar <jan.molnar@abds.de>
//
//    This file is part of JCQ.
//    JCQ is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 2 of the License, or
//    (at your [option]) any later version.
//    JCQ is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//    You should have received a copy of the GNU General Public License
//    along with JCQ. If not, see <http://www.gnu.org/licenses/>.
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
namespace JCsTools.JCQ.IcqInterface
{
  public class TcpContext : ITcpContext, IDisposable
  {
    private System.Net.Sockets.TcpClient _Client;
    private readonly System.Object _lock = new System.Object();
    private bool _IsConnected;

    private long _BytesReceived;
    private long _BytesSent;

    private readonly string _Id = string.Concat("TcpContext:", Guid.NewGuid.ToString);

    private bool _ConnectionCloseExpected;

    public string ITcpContext.Id {
      get { return _Id; }
    }

    public bool ITcpContext.IsConnected {
      get { return _IsConnected; }
    }

    public long ITcpContext.BytesReceived {
      get { return _BytesReceived; }
    }

    public long ITcpContext.BytesSent {
      get { return _BytesSent; }
    }

    public bool ITcpContext.ConnectionCloseExpected {
      get { return _ConnectionCloseExpected; }
    }

    public void ITcpContext.SetCloseExpected()
    {
      lock (_lock) {
        _ConnectionCloseExpected = true;
        Debug.WriteLine(string.Format("{0} expecting disconnect.", Environment.TickCount), _Id);
      }
    }

    public void ITcpContext.SetCloseUnexpected()
    {
      lock (_lock) {
        _ConnectionCloseExpected = false;
        Debug.WriteLine(string.Format("{0} expecting alive.", Environment.TickCount), _Id);
      }
    }

    public void ITcpContext.Connect(System.Net.IPEndPoint endPoint)
    {
      System.Net.Sockets.NetworkStream stream;
      ReceiveInfo info;

      Debug.WriteLine(string.Format("Connecting to {0}.", endPoint), _Id);

      _Client = new System.Net.Sockets.TcpClient();
      _Client.Connect(endPoint);

      _IsConnected = true;

      stream = _Client.GetStream();

      info = new ReceiveInfo(stream);

      stream.BeginRead(info.buffer, 0, ReceiveInfo.BUFFERSIZE, ReadCallback, info);
    }

    public void ITcpContext.Disconnect()
    {
      if (!_IsConnected)
        return;

      _Client.Close();
      OnDisconnected();
    }

    private void ReadCallback(IAsyncResult ar)
    {
      ReceiveInfo info = (ReceiveInfo)ar.AsyncState;
      List<byte> data;

      try {
        lock (_lock) {
          int dataSize;

          dataSize = info.Stream.EndRead(ar);
          data = new List<byte>(info.buffer);
          data = data.GetRange(0, dataSize);

          while (info.Stream.DataAvailable) {
            byte[] buffer = new byte[ReceiveInfo.BUFFERSIZE - 1] {
              
            };

            dataSize = info.Stream.Read(buffer, 0, ReceiveInfo.BUFFERSIZE);
            data.AddRange(new List<byte>(buffer).GetRange(0, dataSize));
          }

          info = new ReceiveInfo(info.Stream);

          if (data.Count > 0) {
            info.Stream.BeginRead(info.buffer, 0, ReceiveInfo.BUFFERSIZE, ReadCallback, info);

            System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(AsyncRaiseDataReceived), data);
          } else {
            Debug.WriteLine("Transfer loop ended with no data.", _Id);
            OnDisconnected();
          }
        }
      } catch (Exception ex) {
        JCsTools.Core.Kernel.Exceptions.PublishException(ex);
        OnDisconnected();
      }
    }

    private void AsyncRaiseDataReceived(object state)
    {
      List<byte> data;

      try {
        data = (List<byte>)state;

        Threading.Interlocked.Add(_BytesReceived, data.Count);

        OnDataReceived(data);
      } catch (Exception ex) {
        Core.Kernel.Exceptions.PublishException(ex);
        OnDisconnected();
      }
    }

    public void ITcpContext.SendData(System.Collections.Generic.List<byte> data)
    {
      if (data == null)
        throw new ArgumentNullException("data");
      if (data.Count == 0)
        throw new ArgumentException("List<Byte> does not contain data.");

      byte[] buffer;

      try {
        buffer = data.ToArray;
        lock (_lock) {
          System.Net.Sockets.NetworkStream stream;

          stream = _Client.GetStream;

          stream.Write(buffer, 0, buffer.Length);
        }

        Threading.Interlocked.Add(_BytesSent, buffer.Length);
      } catch {
        OnDisconnected();
        throw;
      }
    }

    private void OnDataReceived(IEnumerable<byte> data)
    {
      if (DataReceived != null) {
        DataReceived(this, new DataReceivedEventArgs(data));
      }
    }

    private void OnConnected(System.Net.IPEndPoint endpoint)
    {
      if (Connected != null) {
        Connected(this, new ConnectedEventArgs(endpoint));
      }
    }

    private void OnDisconnected()
    {
      bool expected;

      if (!_IsConnected)
        return;

      _IsConnected = false;
      expected = ConnectionCloseExpected;

      if (expected) {
        Debug.WriteLine(string.Format("{0} Connection closed (expected).", Environment.TickCount), _Id);
      } else {
        Debug.WriteLine(string.Format("{0} Connection closed (unexpected).", Environment.TickCount), _Id);
      }

      if (Disconnected != null) {
        Disconnected(this, new DisconnectEventArgs(expected));
      }
    }

    public event DataReceivedEventHandler DataReceived;
    public delegate void DataReceivedEventHandler(object sender, DataReceivedEventArgs e);

    public event ConnectedEventHandler Connected;
    public delegate void ConnectedEventHandler(object sender, ConnectedEventArgs e);

    public event DisconnectedEventHandler Disconnected;
    public delegate void DisconnectedEventHandler(object sender, DisconnectEventArgs e);

    private bool disposedValue = false;

    // IDisposable
    protected virtual void Dispose(bool disposing)
    {
      if (!this.disposedValue) {
        if (disposing) {
          if (_Client != null)
            _Client.Close();
        }
      }
      this.disposedValue = true;
    }

#region  IDisposable Support 
    // This code added by Visual Basic to correctly implement the disposable pattern.
    public void IDisposable.Dispose()
    {
      // Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
      Dispose(true);
      GC.SuppressFinalize(this);
    }
#endregion

  }

  internal class ReceiveInfo
  {
    public const int BUFFERSIZE = 2048;

    public ReceiveInfo(System.Net.Sockets.NetworkStream stream)
    {
      buffer = new byte[BUFFERSIZE - 1] {
        
      };
      _Stream = stream;
    }

    private System.Net.Sockets.NetworkStream _Stream;
    public System.Net.Sockets.NetworkStream Stream {
      get { return _Stream; }
    }


    internal byte[] buffer;
  }
}

