﻿//*********************************************************
//
//    Copyright © Organized Chaos Technologies Inc. 2015 All rights reserved.
//	  Technical Contact: Rahim Bhatia, rahim@organizedchaostech.com
//	  http://www.organizedchaostech.com
//
//*********************************************************

using EasySoft.Helper;
using System;
using System.IO;
using System.Web.Mvc;

namespace eCMS.Shared
{
    public class ImageResult : ActionResult
    {
        public String ContentType { get; set; }
        public byte[] ImageBytes { get; set; }
        public String SourceFilename { get; set; }

        //This is used for times where you have a physical location
        public ImageResult(String sourceFilename, String contentType)
        {
            SourceFilename = sourceFilename;
            ContentType = contentType;
        }

        //This is used for when you have the actual image in byte form
        //  which is more important for this post.
        public ImageResult(byte[] sourceStream, String contentType)
        {
            ImageBytes = sourceStream;
            ContentType = contentType;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;
            try
            {
                //response.Clear();
                //response.Cache.SetCacheability(HttpCacheability.NoCache);
                //response.ContentType = ContentType;

                //Check to see if this is done from bytes or physical location
                //  If you're really paranoid you could set a true/false flag in
                //  the constructor.
                if (ImageBytes != null)
                {
                    var stream = new MemoryStream(ImageBytes);
                    stream.WriteTo(response.OutputStream);
                    stream.Dispose();
                }
                else if (SourceFilename.IsNotNullOrEmpty())
                {
                    response.TransmitFile(SourceFilename);
                }
            }
            catch
            {
                if (SourceFilename.IsNotNullOrEmpty())
                {
                    response.TransmitFile(SourceFilename);
                }
            }
        }
    }
}