using System;

namespace MayCat
{
    [Serializable]
    public class DWord : DataTypeBase
    {
        #region Constructors

        public DWord() : base("DWord", 32) { RequireByteLength = 4; }

        #endregion

        #region Methods

        /// <summary>
        /// Hàm chuyển đổi chuỗi byte thành giá trị
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="pos"></param>
        /// <param name="bit"></param>
        /// <returns></returns>
        public override string ConvertToValue(byte[] buffer, double gain, double offset, int pos = 0, int bit = 0, ByteOrder byteOrder = ByteOrder.ABCD)
        {
            //string abcd = (ByteHelper.GetDWordAt(buffer, pos, ByteOrder.ABCD) * gain + offset).ToString();
            //string badc = (ByteHelper.GetDWordAt(buffer, pos, ByteOrder.BADC) * gain + offset).ToString();
            //string cdab = (ByteHelper.GetDWordAt(buffer, pos, ByteOrder.CDAB) * gain + offset).ToString();
            //string dcba = (ByteHelper.GetDWordAt(buffer, pos, ByteOrder.DCBA) * gain + offset).ToString();
            return (ByteHelper.GetDWordAt(buffer, pos, byteOrder) * gain + offset).ToString();
        }

        /// <summary>
        /// Hàm kiểm tra địa chỉ của <see cref="ITagCore"/> có phù hợp với <see cref="IDataType"/> hay không
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public override bool TryParseToByteArray(object value, double gain, double offset, out byte[] buffer, ByteOrder byteOrder = ByteOrder.ABCD)
        {
            buffer = new byte[RequireByteLength];
            if (value == null)
                return false;
            if (double.TryParse(value.ToString(), out double dResult))
            {
                dResult = (dResult - offset) / gain;
                if (uint.TryParse(dResult.ToString(), out uint result))
                {
                    ByteHelper.SetDWordAt(buffer, 0, result, byteOrder);
                    return true;
                }
            }
            return false;
        }

        #endregion
    }
}
