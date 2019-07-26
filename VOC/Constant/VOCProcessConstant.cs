using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VOC.Constant
{
    public class VOCProcessConstant
    {

        #region Common
        public const string TypeVOC = "VOC";
        public const string STEP = "STEP";
        public const string PARENT = "PARENT";
        public const string DuplicateValidate = "Thông tin đã tồn tại trên hệ thống";
        public const string ErrorValidate = "Error Validate";
        public const int StatusFail = 400;
        public const string TitleValidate = "Title Validate";
        public const string NullNameProcessName = "Tên quy trình không được để trống";
        public const string NullCodeVOCProcess = "Mã quy trình không được để trống";
        public const string NullTypeVOCProcess = "Loại quy trình không được để trống";
        public const string MinVOCProcessCode = "Mã đơn vị tối thiểu có 3 ký tự";
        public const string FormatVOCProcessCode = "Mã đơn vị chỉ được chứa các ký tự số, chữ (a->z, A->Z, 0-9)";
        #endregion

        #region SearchVOCProcess
        #endregion

        #region CopyVOCProcess
        #endregion

        #region AddVOCProcess
        public const int AddVOCProcessFail = 0;
        public const int AddVOCProcessSuccess = 1;
        public const int AddDuplicateCode = 2;
        public const string ErrorKeyAddDuplicateCode = "error.voc.VOCProcessCode";
        public const string ErrorKeyAddFail = "error.voc.addFail";
        public const string MessageAddDuplicateCode = "Thông tin đã tồn tại trên hệ thống";
        public const string MessageAddFail = "Lỗi không hợp lệ";
        public const string MessageAddSuccess = "Lưu thông tin thành công";
        public const string NullStepName = "Tên bước không được để trống";
        
        #endregion

        #region EditVOCProcess
        public const int EditVOCProcessFail = 0;
        public const int EditVOCProcessSuccess = 1;
        public const int EditDuplicateCode = 2;

        public const string NeedVOCProcessCode = "Mã quy trình chưa được gửi lên Server";
        public const string NeedVersion = "Mã phiên bản chưa được gửi lên Server";
        #endregion

        #region DeleteVOCProcess
        public const int DeleteVOCProcessFail = 0;
        public const int DeleteVOCProcessSuccess = 1;
        public const int DeleteDuplicateCode = 2;
        #endregion

        #region ConditionStepVOCProcess
        #endregion

        #region SearchVersionVOCProcess
        #endregion

        #region ConfigFormVOCProcess
        #endregion


        #region ConfigVOCProcess
        #endregion

        #region ConfigStepVOCProcess
        #endregion

        #region Message
        public const int StatusCodeSuccess = 200;
        public const string MessageSuccess = "Success";

        public const string MS0001 = "Không có kết quả thỏa mãn điều kiện nhập";
        public const string MS0002 = "Xóa thông tin đã chọn";
        public const string MS0003 = "Xóa thành công";
        public const string MS0004 = "Không được xóa quy trình này";
        public const string MS0005 = "Hủy thông tin đã nhập";
        public const string MS0006 = "Đã có phiên bản hoạt động.Bạn có muốn chuyển trạng thái?";
        #endregion
        public const int SwitchStatusFail = 0;
        public const int SwitchStatusSuccess = 1;
        public const int SwitchStatusNotChange = 2;
        public const int SwitchStatusNotFound = 3;
        public const string MSSwitchStatusFail = "Lỗi không xác định";
        public const string MSSwitchStatusSuccess = "Thay đổi trạng thái phiên bản thành công";
        public const string MSSwitchStatusNotChange = "Trạng thái hiện tại của phiên bản không thay đổi";
        public const string MSSwitchStatusNotFound = "Không tìm thấy phiên bản hợp lệ";
        public const string MSSwitchStatusMissCodeVOC = "Không tìm thấy mã phiên bản";


        public const string MSNeedStatus = "Trạng thái truyền lên không hợp lệ";

        public const string ErrorKeySwitchStatusFail = "error.voc.switchfail";
        //public const string ErrorKeySwitchStatusSuccess = "error.voc.switchfail";
        public const string ErrorKeyMSSwitchStatusNotChange = "error.voc.switch_not_change";
        public const string ErrorKeyMSSwitchStatusNotFound = "error.voc.switch_not_found";
        public const string ErrorKeyMissCodeVOC = "error.voc.voc_code";
        public const string ErrorKeyEditFail = "error.voc.edit_fail";

        public const string FieldVOCProcessCode = "VOCProcessCode";
        public const string FieldVersion = "Version";
        public const string FieldIsActive = "IsActive";
        public const string FieldVOCProcessType = "VOCProcessType";
        public const string FieldVOCProcessName = "VOCProcessName";


        public const string MSEditSuccess = "Sửa thông tin thành công";
        public const string MSEditFail = "Có lỗi xảy ra khi sửa thông tin quy trình";
        public const string MSFail = "Sửa thông tin thành công";

    }
}
