﻿//                             u2pa
//
//    A command line interface for Top Universal Programmers
//
//    Copyright (C) Elgen };-) aka Morten Overgaard 2012
//
//    This file is part of u2pa.
//
//    u2pa is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    Foobar is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with u2pa. If not, see <http://www.gnu.org/licenses/>.

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace U2Pa.Lib
{
  public class ZIFSocket
  {
    private readonly BitArray pins;

    public ZIFSocket(int size, byte[] initialTopbytes)
    : this(size)
    {
      SwallowTopBytes(initialTopbytes);
    }

    public ZIFSocket(int size)
    {
      pins = new BitArray(size + 1);
    }

    public bool this[int i]
    {
      get { return pins[i]; }
      set { pins[i] = value; }
    }

    public void SetAll(bool value)
    {
      pins.SetAll(value);
    }

    public void SetEpromAddress(Eproms.Eprom eprom, int address)
    {
      var translator = new PinNumberTranslator(eprom.DilType, eprom.Placement);
      var bitAddress = new BitArray(new[] { address });
      for (var i = 0; i < eprom.AddressPins.Length; i++)
        pins[translator.ToZIF(eprom.AddressPins[i])] = bitAddress[i];
    }

    internal void SetEpromData(Eproms.Eprom eprom, byte[] data)
    {
      var translator = new PinNumberTranslator(eprom.DilType, eprom.Placement);
      var bitAddress = new BitArray(data);
      for (var i = 0; i < eprom.DataPins.Length; i++)
        pins[translator.ToZIF(eprom.DataPins[i])] = bitAddress[i];
    }

    public int GetEpromAddress(Eproms.Eprom eprom)
    {
      var translator = new PinNumberTranslator(eprom.DilType, eprom.Placement);
      var acc = 0;
      for (var i = 0; i < eprom.AddressPins.Length; i++)
      {
        acc |= pins[translator.ToZIF(eprom.AddressPins[i])] ? 1 << i : 0;
      }
      return acc;
    }

    public IEnumerable<byte> GetEpromData(Eproms.Eprom eprom)
    {
      var translator = new PinNumberTranslator(eprom.DilType, eprom.Placement);
      var readByte = new BitArray(eprom.DataPins.Length);
      for (var i = 0; i < eprom.DataPins.Length; i++)
        readByte[i] = pins[translator.ToZIF(eprom.DataPins[i])];
      return readByte.ToBytes();
    }

    private void SwallowTopBytes(byte[] bytes)
    {
      SwallowTopByte(bytes[0], 1, 8, 20);
      SwallowTopByte(bytes[1], 9, 16, 20);
      SwallowTopByte(bytes[2], 17, 25, 20);
      SwallowTopByte(bytes[3], 26, 33, 20);
      SwallowTopByte(bytes[4], 34, 40, 20);
    }

    private void SwallowTopByte(byte b, int from, int to, int skip)
    {
      var skipOffset = 0;
      for(var i = from; i <= to; i++)
      {
        if (i == skip)
          skipOffset = 1;
        pins[i + skipOffset] = b%2 != 0;
        b = (byte)(b >> 1);
      }
    }

    public byte[] ToTopBytes()
    {
      return new[]
               {
                 ToTopByte(1, 8, 20),
                 ToTopByte(9, 16, 20),
                 ToTopByte(17, 25, 20),
                 ToTopByte(26, 33, 20),
                 ToTopByte(34, 40, 20)
               };
    }

    private byte ToTopByte(int from, int to, int skip)
    {
      byte acc = 0x00;
      var j = 0;
      for (var i = from; i <= to; i++)
      {
        if (i == skip) continue;
        if (pins[i]) acc |= (byte)(0x01 << j);
        j++;
      }
      return acc;
    }
  }
}