﻿/*
 * Copyright (c) 2016, Will Strohl
 * All rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without modification, 
 * are permitted provided that the following conditions are met:
 * 
 * Redistributions of source code must retain the above copyright notice, this list 
 * of conditions and the following disclaimer.
 * 
 * Redistributions in binary form must reproduce the above copyright notice, this 
 * list of conditions and the following disclaimer in the documentation and/or 
 * other materials provided with the distribution.
 * 
 * Neither the name of Will Strohl, nor the names of its contributors may be used 
 * to endorse or promote products derived from this software without specific prior 
 * written permission.
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND 
 * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED 
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. 
 * IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, 
 * INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, 
 * BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, 
 * DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF 
 * LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE 
 * OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF 
 * ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using System.Collections.Generic;
using System.Linq;
using DotNetNuke.Data;

namespace WillStrohl.Modules.CodeCamp.Entities
{
    public class SpeakerInfoRepository
    {
        public void CreateItem(SpeakerInfo i)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<SpeakerInfo>();
                rep.Insert(i);
            }
        }

        public void DeleteItem(int itemId, int registrationId)
        {
            var i = GetItem(itemId, registrationId);
            DeleteItem(i);
        }

        public void DeleteItem(SpeakerInfo i)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<SpeakerInfo>();
                rep.Delete(i);
            }
        }

        public IEnumerable<SpeakerInfo> GetItems(int codeCampId)
        {
            IEnumerable<SpeakerInfo> i;
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<SpeakerInfo>();
                i = rep.Get(codeCampId);
            }
            return i;
        }

        public SpeakerInfo GetItem(int itemId, int codeCampId)
        {
            SpeakerInfo i = null;
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<SpeakerInfo>();
                i = rep.GetById(itemId, codeCampId);
            }
            return i;
        }

        public SpeakerInfo GetItemByRegistrationId(int codeCampId, int registrationId)
        {
            SpeakerInfo i = null;

            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<SpeakerInfo>();

                i = rep.Find("WHERE CodeCampId = @0 AND RegistrationId = @1", codeCampId, registrationId).FirstOrDefault();
            }

            return i;
        }

        public void UpdateItem(SpeakerInfo i)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<SpeakerInfo>();
                rep.Update(i);
            }
        }
    }
}