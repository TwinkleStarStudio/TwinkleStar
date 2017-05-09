using MrHai.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MrHai.Core
{
    public abstract partial class MrHaiService
    {

        public int GetMaxAttachmentID()
        {
            var data = AttachmentRepository.GetModel();
            if (data==null||data.Count()<=0)
            {
                return 0;
            }
            else
            {
                return data.Max(it => it.Id);
            }
        }

        public int InsertAttachment(Attachment model)
        {
            model.CreateTime = DateTime.Now;
            return AttachmentRepository.Insert(model);
        }

        public string  GetModelCode(Attachment model)
        {
            var data = AttachmentRepository.GetModel(it=>it.Extension==model.Extension&&it.FileName==model.FileName&&it.Size==model.Size);
            if (data==null||data.Count()<=0)
            {
                return string.Empty;
            }
            else
            {
                return data.FirstOrDefault().Code;
            }
        }
    }
}
