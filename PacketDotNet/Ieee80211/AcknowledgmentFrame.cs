/*
This file is part of PacketDotNet

PacketDotNet is free software: you can redistribute it and/or modify
it under the terms of the GNU Lesser General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

PacketDotNet is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License
along with PacketDotNet.  If not, see <http://www.gnu.org/licenses/>.
*/
/*
 * Copyright 2012 Alan Rushforth <alan.rushforth@gmail.com>
 */

using System.Net.NetworkInformation;
using PacketDotNet.Utils;

namespace PacketDotNet.Ieee80211
{
    /// <summary>
    /// Format of an ACK frame
    /// </summary>
    public sealed class AcknowledgmentFrame : MacFrame
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="byteArraySegment">
        /// A <see cref="ByteArraySegment" />
        /// </param>
        public AcknowledgmentFrame(ByteArraySegment byteArraySegment)
        {
            Header = new ByteArraySegment(byteArraySegment);

            FrameControl = new FrameControlField(FrameControlBytes);
            Duration = new DurationField(DurationBytes);
            ReceiverAddress = GetAddress(0);

            Header.Length = FrameSize;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AcknowledgmentFrame" /> class.
        /// </summary>
        /// <param name='receiverAddress'>
        /// Receiver address.
        /// </param>
        public AcknowledgmentFrame(PhysicalAddress receiverAddress)
        {
            FrameControl = new FrameControlField();
            Duration = new DurationField();
            ReceiverAddress = receiverAddress;

            FrameControl.SubType = FrameControlField.FrameSubTypes.ControlAck;
        }

        /// <summary>
        /// Length of the frame
        /// </summary>
        public override int FrameSize => MacFields.FrameControlLength +
                                         MacFields.DurationIDLength +
                                         MacFields.AddressLength;

        /// <summary>
        /// Receiver address
        /// </summary>
        public PhysicalAddress ReceiverAddress { get; set; }

        /// <summary>
        /// Writes the current packet properties to the backing ByteArraySegment.
        /// </summary>
        public override void UpdateCalculatedValues()
        {
            if (Header == null || Header.Length > Header.BytesLength - Header.Offset || Header.Length < FrameSize)
            {
                Header = new ByteArraySegment(new byte[FrameSize]);
            }

            FrameControlBytes = FrameControl.Field;
            DurationBytes = Duration.Field;
            SetAddress(0, ReceiverAddress);
        }

        /// <summary>
        /// Returns a string with a description of the addresses used in the packet.
        /// This is used as a component of the string returned by ToString().
        /// </summary>
        /// <returns>
        /// The address string.
        /// </returns>
        protected override string GetAddressString()
        {
            return $"RA {ReceiverAddress}";
        }
    }
}