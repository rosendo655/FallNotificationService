using System;
using System.Collections.Generic;
using System.Text;

namespace FallNotificationService
{
    public class FallEventData
    {
        public IEnumerable<IEnumerable<float>> FrameBuffer { get; set; }
    }
}
