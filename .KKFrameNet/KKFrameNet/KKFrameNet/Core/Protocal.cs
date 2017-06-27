using System;

namespace KK.Frame.Net
{
	public interface Protocal
	{
        ByteBuffer TranslateFrame (byte[] cbSrc, int nLen); 
	    int HeaderLen();
	}
}

