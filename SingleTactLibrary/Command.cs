//-----------------------------------------------------------------------------
//                                                                            
//  Copyright (c) 2015 All Right Reserved                                      
//  Pressure Profile Systems                                                   
//  www.pressureprofile.com                                                    
//  V1.0                                                         
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SingleTactLibrary
{
   public class SerialCommand
   {
      const int TIMEOUT = 100;

      /// <summary>
      /// Generate raw command packet
      /// </summary>
      /// <param name="i2cAddress"></param>
      /// <param name="ID">Command ID</param>
      /// <param name="writeLocation"></param>
      /// <param name="data"></param>
      /// <returns></returns>
      public static byte[] GenerateWriteCommand(byte i2cAddress, byte ID, byte writeLocation, byte[] data)
      {
         byte[] command = new byte[data.Length+15];
         for (int i = 0; i < 4; i++)
            command[i] = 0xFF;

         command[4] = i2cAddress;
         command[5] = TIMEOUT;
         command[6] = ID;
         command[7] = 0x02;
         command[8] = writeLocation;
         command[9] = (byte)data.Length;
         command[10 + data.Length] = 0xFF;

         for (int i = 0; i < data.Length; i++)
            command[10 + i] = data[i];

         for (int i = 0; i < 4; i++)
            command[11 + i + data.Length] = 0xFE;

         return command;
      }


      /// <summary>
      /// Generate raw command packet
      /// </summary>
      /// <param name="i2cAddress"></param>
      /// <param name="ID">Command ID</param>
      /// <param name="writeLocation"></param>
      /// <param name="data"></param>
      /// <returns></returns>
      public static byte[] GenerateWriteCalCommand(byte i2cAddress, byte ID, byte writeLocation, byte[] data)
      {
         byte[] command = new byte[data.Length + 15];
         for (int i = 0; i < 4; i++)
            command[i] = 0xFF;

         command[4] = i2cAddress;
         command[5] = TIMEOUT;
         command[6] = ID;
         command[7] = 0x04;
         command[8] = writeLocation;
         command[9] = (byte)data.Length;
         command[10 + data.Length] = 0xFF;

         for (int i = 0; i < data.Length; i++)
            command[10 + i] = data[i];

         for (int i = 0; i < 4; i++)
            command[11 + i + data.Length] = 0xFE;

         return command;
      }

      /// <summary>
      /// Generate raw command packet
      /// </summary>
      /// <param name="i2cAddress"></param>
      /// <param name="ID">Command ID</param>
      /// <param name="readLocation"></param>
      /// <param name="numToRead"></param>
      /// <returns></returns>
      public static byte[] GenerateReadCommand(byte i2cAddress, byte ID, byte readLocation, byte numToRead)
      {
         byte[] command = new byte[16];
         for (int i = 0; i < 4; i++)
            command[i] = 0xFF;

         command[4] = i2cAddress;
         command[5] = TIMEOUT;
         command[6] = ID;
         command[7] = 0x01;
         command[8] = readLocation;
         command[9] = numToRead;
         command[10] = 0xFF;

         for (int i = 0; i < 4; i++)
            command[11 + i] = 0xFE;

         return command;
      }


      /// <summary>
      /// Generate raw command packet
      /// </summary>
      /// <param name="i2cAddress"></param>
      /// <param name="ID">Command ID</param>
      /// <param name="writeLocation"></param>
      /// <param name="data"></param>
      /// <returns></returns>
      public static byte[] GenerateToggleCommand(byte i2cAddress, byte ID, byte writeLocation, byte data)
      {
         /*
         byte[] command = new byte[16];
         for (int i = 0; i < 4; i++)
            command[i] = 0xFF;

         command[4] = i2cAddress;
         command[5] = TIMEOUT;
         command[6] = ID;
         command[7] = 0x03;
         command[8] = data;
         command[9] = 1;
         command[10] = 0xFF;

         for (int i = 0; i < 4; i++)
            command[11 + i] = 0xFE;

         return command;*/

         byte[] command = new byte[16 + 15];
         for (int i = 0; i < 4; i++)
            command[i] = 0xFF;

         command[4] = i2cAddress;
         command[5] = TIMEOUT;
         command[6] = ID;
         command[7] = 0x03;
         command[8] = data;
         command[9] = (byte)16;
         command[10 + 16] = 0xFF;

         for (int i = 0; i < 16; i++)
            command[10 + i] = 0x07;

         for (int i = 0; i < 4; i++)
            command[11 + i + 16] = 0xFE;

         return command;


         
      }



   }
}
