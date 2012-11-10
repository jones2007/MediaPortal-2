using System;
using System.IO;
using System.Runtime.InteropServices;
using DirectShow;
using DirectShow.BaseClasses;
using Sonic;

namespace DxPlayer
{
  [ComVisible(true)]
  [Guid("BD32F0D8-1E0D-46BE-8DE6-E3C4C5746633")]
  public interface INetStreamSourceFilter
  {
    int SetSourceStream(Stream sourceStream);
  }

  [ComVisible(true)]
  [Guid("19651B59-6AD6-4FD7-882A-914ED5592BFA")]
  [ClassInterface(ClassInterfaceType.None)]
  public class NetStreamSourceFilter : BaseFilter, INetStreamSourceFilter//, DirectShowLib.IBaseFilter
  {
    protected Stream sourceStream = null;
    protected NetStreamOutputPin outputPin = null;

    public NetStreamSourceFilter()
      : base("Net Stream Source Filter")
    {
    }

    ~NetStreamSourceFilter()
    {
      sourceStream = null;
    }

    protected override int OnInitializePins()
    {
      outputPin = new NetStreamOutputPin("Output", this, sourceStream);
      AddPin(outputPin);
      return NOERROR;
    }

    public int SetSourceStream(Stream sourceStream)
    {
      if (this.sourceStream != null)
        return E_UNEXPECTED;
      else
      {
        this.sourceStream = sourceStream;
        return NOERROR;
      }
    }
    /*
    int DirectShowLib.IBaseFilter.EnumPins(out DirectShowLib.IEnumPins ppEnum)
    {
      ppEnum = null;
      return NOERROR;
    }

    int DirectShowLib.IBaseFilter.FindPin(string Id, out DirectShowLib.IPin ppPin)
    {
      ppPin = null;
      return NOERROR;
    }

    int DirectShowLib.IBaseFilter.GetClassID(out Guid pClassID)
    {
      return GetClassID(out pClassID);
    }

    int DirectShowLib.IBaseFilter.GetState(int dwMilliSecsTimeout, out DirectShowLib.FilterState filtState)
    {
      FilterState fs;
      int hr = GetState(dwMilliSecsTimeout, out fs);
      filtState = (DirectShowLib.FilterState)(int)fs;
      return hr;
    }

    int DirectShowLib.IBaseFilter.GetSyncSource(out DirectShowLib.IReferenceClock pClock)
    {
      IntPtr ss = IntPtr.Zero;
      pClock = null;
      int hr = GetSyncSource(out ss);
      if (hr == NOERROR && ss != IntPtr.Zero)
        pClock = Marshal.GetObjectForIUnknown(ss) as DirectShowLib.IReferenceClock;
      return hr;
    }

    int DirectShowLib.IBaseFilter.JoinFilterGraph(DirectShowLib.IFilterGraph pGraph, string pName)
    {
      if (pGraph == null)
      {
        if (outputPin != null)
        {
          RemovePin(outputPin);
          outputPin = null;
        }
        if (sourceStream != null)
        {
          sourceStream.Close();
          sourceStream = null;
        }
        return JoinFilterGraph(IntPtr.Zero, pName);
      }
      else
        return JoinFilterGraph(Marshal.GetComInterfaceForObject(pGraph, typeof(IFilterGraph)), pName);
    }

    int DirectShowLib.IBaseFilter.Pause()
    {
      return Pause();
    }

    int DirectShowLib.IBaseFilter.QueryFilterInfo(out DirectShowLib.FilterInfo pInfo)
    {
      pInfo = default(DirectShowLib.FilterInfo);
      FilterInfo fi;
      int hr = QueryFilterInfo(out fi);
      if (hr == NOERROR)
        pInfo = new DirectShowLib.FilterInfo() { achName = fi.achName };
      return hr;
    }

    int DirectShowLib.IBaseFilter.QueryVendorInfo(out string pVendorInfo)
    {
      return QueryVendorInfo(out pVendorInfo);
    }

    int DirectShowLib.IBaseFilter.Run(long tStart)
    {
      return Run(tStart);
    }

    int DirectShowLib.IBaseFilter.SetSyncSource(DirectShowLib.IReferenceClock pClock)
    {
      if (pClock == null)
        return SetSyncSource(IntPtr.Zero);
      else
        return SetSyncSource(Marshal.GetComInterfaceForObject(pClock, typeof(IReferenceClock)));
    }

    int DirectShowLib.IBaseFilter.Stop()
    {
      return Stop();
    }

    int DirectShowLib.IMediaFilter.GetClassID(out Guid pClassID)
    {
      return GetClassID(out pClassID);
    }

    int DirectShowLib.IMediaFilter.GetState(int dwMilliSecsTimeout, out DirectShowLib.FilterState filtState)
    {
      FilterState fs;
      int hr = GetState(dwMilliSecsTimeout, out fs);
      filtState = (DirectShowLib.FilterState)(int)fs;
      return hr;
    }

    int DirectShowLib.IMediaFilter.GetSyncSource(out DirectShowLib.IReferenceClock pClock)
    {
      IntPtr ss = IntPtr.Zero;
      pClock = null;
      int hr = GetSyncSource(out ss);
      if (hr == NOERROR && ss != IntPtr.Zero)
        pClock = Marshal.GetObjectForIUnknown(ss) as DirectShowLib.IReferenceClock;
      return hr;
    }

    int DirectShowLib.IMediaFilter.Pause()
    {
      return Pause();
    }

    int DirectShowLib.IMediaFilter.Run(long tStart)
    {
      return Run(tStart);
    }

    int DirectShowLib.IMediaFilter.SetSyncSource(DirectShowLib.IReferenceClock pClock)
    {
      if (pClock == null)
        return SetSyncSource(IntPtr.Zero);
      else
        return SetSyncSource(Marshal.GetComInterfaceForObject(pClock, typeof(IReferenceClock)));
    }

    int DirectShowLib.IMediaFilter.Stop()
    {
      return Stop();
    }

    int DirectShowLib.IPersist.GetClassID(out Guid pClassID)
    {
      return GetClassID(out pClassID);
    }*/
  }

  [ComVisible(true)]
  [Guid("8CF6F982-E2A4-4DC4-A437-8E9F8533EA1D")]
  public class NetStreamOutputPin : BasePin, IAsyncReader
  {
    protected Stream sourceStream = null;

    public NetStreamOutputPin(string _name, BaseFilter _filter, Stream sourceStream)
      : base(_name, _filter, _filter.FilterLock, PinDirection.Output)
    {
      if (sourceStream == null)
      {
        throw new ArgumentException("Paramater cannot be null!", "sourceStream");
      }
      else
      {
        this.sourceStream = sourceStream;
      }
    }

    public override int BeginFlush()
    {
      return E_UNEXPECTED;
    }

    public override int EndFlush()
    {
      return E_UNEXPECTED;
    }

    public override int GetMediaType(int iPosition, ref AMMediaType pMediaType)
    {
      lock (m_Filter.FilterLock)
      {
        if (iPosition < 0)
        {
          return E_INVALIDARG;
        }
        if (iPosition > 0)
        {
          return VFW_S_NO_MORE_ITEMS;
        }
        return GetMediaType(ref pMediaType);
      }
    }

    public override int CheckMediaType(AMMediaType pmt)
    {
      lock (m_Filter.FilterLock)
      {
        AMMediaType mt = null;
        AMMediaType.Init(ref mt);
        try
        {
          GetMediaType(ref mt);
          if (AMMediaType.AreEquals(mt, pmt))
          {
            return NOERROR;
          }
        }
        finally
        {
          AMMediaType.Free(ref mt);
          mt = null;
        }
      }
      return E_FAIL;
    }

    int GetMediaType(ref AMMediaType pMediaType)
    {
      if (sourceStream == null) return E_UNEXPECTED;

      pMediaType.majorType = MediaType.Stream;
      pMediaType.subType = MediaSubType.Null;
      pMediaType.formatType = FormatType.None;
      pMediaType.temporalCompression = false;
      pMediaType.sampleSize = 1;

      return NOERROR;
    }

    public override int CheckConnect(ref IPinImpl _pin)
    {
      PinDirection _direction;
      HRESULT hr = (HRESULT)_pin.QueryDirection(out _direction);
      if (hr.Failed) return hr;
      if (_direction == m_Direction)
      {
        return VFW_E_INVALID_DIRECTION;
      }
      return NOERROR;
    }

    public override int CompleteConnect(ref IPinImpl pReceivePin)
    {
      return NOERROR;
    }

    public int RequestAllocator(IntPtr pPreferred, AllocatorProperties pProps, out IntPtr ppActual)
    {
      if (pPreferred != null)
      {
        ppActual = pPreferred;
        if (pProps != null)
          new IMemAllocatorImpl(pPreferred).GetProperties(pProps);
        return S_OK;
      }
      ppActual = IntPtr.Zero;
      return E_FAIL;
    }

    public int Request(IntPtr pSample, IntPtr dwUser)
    {
      throw new NotImplementedException();
    }

    public int WaitForNext(int dwTimeout, out IntPtr ppSample, out IntPtr pdwUser)
    {
      throw new NotImplementedException();
    }

    public int SyncReadAligned(IntPtr pSample)
    {
      throw new NotImplementedException();
    }

    public int SyncRead(long llPosition, int lLength, IntPtr pBuffer)
    {
      byte[] array = new byte[lLength];
      if (sourceStream.Position != llPosition)
      {
        sourceStream.Seek(llPosition, SeekOrigin.Begin);
      }
      int read = sourceStream.Read(array, 0, lLength);
      Marshal.Copy(array, 0, pBuffer, read);
      return NOERROR;
    }

    public int Length(out long pTotal, out long pAvailable)
    {
      pTotal = pAvailable = sourceStream.Length;
      return NOERROR;
    }
  }
}
