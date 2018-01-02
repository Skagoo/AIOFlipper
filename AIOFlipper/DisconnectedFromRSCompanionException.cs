using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AIOFlipper
{
    class DisconnectedFromRSCompanionException: Exception, ISerializable
    {
        public DisconnectedFromRSCompanionException()
        {
            base.GetBaseException();
        }
        public DisconnectedFromRSCompanionException(string message)
        {
            base.GetBaseException();
        }
        public DisconnectedFromRSCompanionException(string message, Exception inner)
        {
            base.GetBaseException();
        }

        // This constructor is needed for serialization.
        protected DisconnectedFromRSCompanionException(SerializationInfo info, StreamingContext context)
        {
            base.GetBaseException();
        }
    }
}
