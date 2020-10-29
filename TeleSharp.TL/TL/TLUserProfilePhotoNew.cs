using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeleSharp.TL;
namespace TeleSharp.TL
{
    [TLObject(-321430132)]
    public class TLUserProfilePhotoNew : TLAbsUserProfilePhoto
    {
        public override int Constructor
        {
            get
            {
                return -321430132;
            }
        }

        public long PhotoId { get; set; }
        public TLAbsFileLocation PhotoSmall { get; set; }
        public TLAbsFileLocation PhotoBig { get; set; }
        public int DcId { get; set; }


        public void ComputeFlags()
        {

        }

        public override void DeserializeBody(BinaryReader br)
        {
            PhotoId = br.ReadInt64();
            PhotoSmall = (TLAbsFileLocation)ObjectUtils.DeserializeObject(br);
            PhotoBig = (TLAbsFileLocation)ObjectUtils.DeserializeObject(br);
            DcId = br.ReadInt32();

        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(PhotoId);
            ObjectUtils.SerializeObject(PhotoSmall, bw);
            ObjectUtils.SerializeObject(PhotoBig, bw);
            bw.Write(DcId);

        }
    }
}
