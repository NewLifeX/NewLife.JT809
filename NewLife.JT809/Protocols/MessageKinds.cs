using System.ComponentModel;

namespace NewLife.JT809.Protocols;

/// <summary>
/// 业务数据类型标识
/// 数据交换一协议规定的业务数据类型名称和标识常量定义见表 73。业务数据类型标识的命名规则如下:
/// a) 上级平台向下级平台发送的请求消息，一般以“DOWN_”开头，以后缀_REQ 结尾;而下级平台向上级平台发送的请求消息一般以“UP_”开头，以后缀_REQ 结尾;
/// b) 当上下级平台之间有应答消息情况下，应答消息可继续沿用对应的请求消息开头标识符，而通过后缀 RSP 来标识结尾。
/// </summary>
public enum MessageKinds : UInt16
{
    #region 链路管理类
    ///<summary>
    ///主链路登录请求消息
    ///UP_CONNECT_REQ
    ///</summary>
    [Description("主链路登录请求消息")]
    主链路登录请求消息 = 0x1001,

    ///<summary>
    ///主链路登录应答消息  
    ///UP_CONNECT_RSP
    ///</summary>
    [Description("主链路登录应答消息")]
    主链路登录应答消息 = 0x1002,

    ///<summary>
    ///主链路注销请求消息 
    ///UP_DISCONNECT_REQ
    ///</summary>
    [Description("主链路注销请求消息")]
    主链路注销请求消息 = 0x1003,
    ///<summary>
    ///主链路注销应答消息 
    ///UP_DISCONNECT_RSP
    ///</summary>
    [Description("主链路注销应答消息")]
    主链路注销应答消息 = 0x1004,
    ///<summary>
    ///主链路连接保持请求消息  
    ///UP_LINKTEST_REQ
    ///</summary>
    [Description("主链路连接保持请求消息")]
    主链路连接保持请求消息 = 0x1005,
    ///<summary>
    ///主链路连接保持应答消息  
    ///UP_LINKTEST_RSP
    ///</summary>
    [Description("主链路连接保持应答消息")]
    主链路连接保持应答消息 = 0x1006,
    ///<summary>
    ///主链路断开通知消息 
    ///UP_DISCONNECT_INFORM
    ///</summary>
    [Description("主链路断开通知消息")]
    主链路断开通知消息 = 0x1007,
    ///<summary>
    ///下级平台主动关闭主从链路通知消息  
    ///UP_CLOSELINK_INFORM
    ///</summary>
    [Description("下级平台主动关闭主从链路通知消息")]
    下级平台主动关闭主从链路通知消息 = 0x1008,
    ///<summary>
    ///从链路连接请求消息  
    ///DOWN_CONNECT_REQ
    ///</summary>
    [Description("从链路连接请求消息")]
    从链路连接请求消息 = 0x9001,
    ///<summary>
    ///从链路连接应答信息 
    ///DOWN_CONNECT_RSP
    ///</summary>
    [Description("从链路连接应答信息")]
    从链路连接应答信息 = 0x9002,
    ///<summary>
    ///从链路注销请求消息  
    ///DOWN_DISCONNECT_REQ
    ///</summary>
    [Description("从链路注销请求消息")]
    从链路注销请求消息 = 0x9003,
    ///<summary>
    ///从链路注销应答消息  
    ///DOWN_DISCONNECT_RSP
    ///</summary>
    [Description("从链路注销应答消息")]
    从链路注销应答消息 = 0x9004,
    ///<summary>
    ///从链路连接保持请求消息  
    ///DOWN_LINKTEST_REQ
    ///</summary>
    [Description("从链路连接保持请求消息")]
    从链路连接保持请求消息 = 0x9005,
    ///<summary>
    ///从链路连接保持应答消息 
    ///DOWN_LINKTEST_RSP
    ///</summary>
    [Description("从链路连接保持应答消息")]
    从链路连接保持应答消息 = 0x9006,
    ///<summary>
    ///从链路断开通知消息  
    ///DOWN_DISCONNECT_INFORM
    ///</summary>
    [Description("从链路断开通知消息")]
    从链路断开通知消息 = 0x9007,
    ///<summary>
    ///上级平台主动关闭链路通知消息
    ///DOWN_CLOSELINK_INFORM
    ///</summary>
    [Description("上级平台主动关闭链路通知消息")]
    上级平台主动关闭链路通知消息 = 0x9008,
    #endregion

    #region 信息统计类
    ///<summary>
    ///接收车辆定位信息数量通知消息
    ///DOWN_TOTAL_RECV_BACK_MSG
    ///</summary>
    [Description("接收车辆定位信息数量通知消息")]
    接收车辆定位信息数量通知消息 = 0x9101,
    /////<summary>
    /////发送车辆定位信息数量通知消息_2019。2019版对0x9101的称呼，方向为主链路上行，数据体与2011版一致
    /////UP_MANAGE_TOTAL_RECV_BACK_MSG
    /////</summary>
    //[Description("发送车辆定位信息数量通知消息_2019")]
    //发送车辆定位信息数量通知消息_2019 = 0x9101,
    ///<summary>
    ///平台链路连接情况与车辆定位消息传输情况上报请求消息_2019
    ///DOWN_TOTAL_RECV_BACK_MSG
    ///</summary>
    [Description("平台链路连接情况与车辆定位消息传输情况上报请求消息")]
    平台链路连接情况与车辆定位消息传输情况上报请求消息_2019 = 0x9102,
    ///<summary>
    ///平台链路连接情况与车辆定位消息传输情况上报应答消息_2019
    ///UP_MANAGE_MSG_RSP
    ///</summary>
    [Description("平台链路连接情况与车辆定位消息传输情况上报应答消息")]
    平台链路连接情况与车辆定位消息传输情况上报应答消息_2019 = 0x1102,
    ///<summary>
    ///上传平台间消息序列号通知消息
    ///UP_MANAGE_MSG_SN_INFORM
    ///</summary>
    [Description("上传平台间消息序列号通知消息_2019")]
    上传平台间消息序列号通知消息_2019 = 0x1103,
    ///<summary>
    ///下发平台间消息序列号通知消息
    ///UP_MANAGE_MSG_SN_INFORM
    ///</summary>
    [Description("下发平台间消息序列号通知消息_2019")]
    下发平台间消息序列号通知消息_2019 = 0x9103,
    #endregion

    #region 车辆动态信息交换
    ///<summary>
    ///主链路动态信息交换消息 
    ///UP_EXG_MSG
    ///</summary>
    [Description("主链路车辆动态信息交换业务")]
    主链路车辆动态信息交换业务 = 0x1200,
    ///<summary>
    ///从链路车辆动态信息交换业务
    ///DOWN_EXG_MSG
    ///</summary>
    [Description("从链路车辆动态信息交换业务")]
    从链路车辆动态信息交换业务 = 0x9200,
    #endregion

    #region 平台间信息交互类
    ///<summary>
    ///主链路平台间信息交互消息  
    ///UP_PLATFORM_MSG
    ///</summary>
    [Description("主链路平台间信息交互消息")]
    主链路平台间信息交互消息 = 0x1300,
    ///<summary>
    ///从链路平台间信息交互业务  
    ///DOWN_PLATFORM_MSG
    ///</summary>
    [Description("从链路平台间信息交互业务")]
    从链路平台间信息交互业务 = 0x9300,
    #endregion

    #region 车辆报警信息交互类(2013)  报警信息交互业务类(2019)
    ///<summary>
    ///主链路报警信息交互消息  
    ///UP_WARN_MSG
    ///</summary>
    [Description("主链路报警信息交互消息")]
    主链路报警信息交互消息 = 0x1400,

    ///<summary>
    ///从链路报警信息交互消息 
    ///DOWN_WARN_MSG
    ///</summary>
    [Description("从链路报警信息交互消息")]
    从链路报警信息交互消息 = 0x9400,
    #endregion

    #region 车辆监管类 
    ///<summary>
    ///主链路车辆监管消息 
    ///UP_CTRL_MSG
    ///</summary>
    [Description("主链路车辆监管消息")]
    主链路车辆监管消息 = 0x1500,

    ///<summary>
    ///从链路车辆监管消息
    ///DOWN_CTRL_MSG
    ///</summary>
    [Description("从链路车辆监管消息")]
    从链路车辆监管消息 = 0x9500,
    #endregion

    #region 车辆静态信息交换类
    ///<summary>
    ///主链路静态信息交换消息 
    ///UP_BASE_MSG
    ///</summary>
    [Description("主链路静态信息交换消息")]
    主链路静态信息交换消息 = 0x1600,
    ///<summary>
    ///从链路静态信息交换消息
    ///DOWN_BASE_MSG
    ///</summary>
    [Description("从链路静态信息交换消息")]
    从链路静态信息交换消息 = 0x9600,
    #endregion

    #region 主链路动态信息交换消息 UP_EXG_MSG
    /// <summary>
    /// 上传车辆注册信息
    /// UP_EXG_MSG_REGISTER
    /// </summary>
    [Description("上传车辆注册信息")]
    上传车辆注册信息 = 0x1201,
    ///<summary>
    ///实时上传车辆定位信息  
    ///UP_EXG_MSG_REAL_LOCATION
    ///</summary>
    [Description("实时上传车辆定位信息")]
    实时上传车辆定位信息 = 0x1202,
    ///<summary>
    ///车辆定位信息自动补报请求消息
    ///UP_EXG_MSG_HISTORY_LOCATION
    ///</summary>
    [Description("车辆定位信息自动补报请求消息")]
    车辆定位信息自动补报请求消息 = 0x1203,
    ///<summary>
    ///启动车辆定位信息交换应答消息	
    ///UP_EXG_MSG_RETURN_STARTUP_ACK
    ///</summary>
    [Description("启动车辆定位信息交换应答消息")]
    启动车辆定位信息交换应答消息 = 0x1205,
    ///<summary>
    ///结束车辆定位信息交换应答	
    ///UP_EXG_MSG_RETURN_END_ACK
    ///</summary>
    [Description("结束车辆定位信息交换应答消息")]
    结束车辆定位信息交换应答消息 = 0x1206,
    ///<summary>
    ///申请交换指定车辆定位信息请求	
    ///UP_EXG_MSG_APPLY_FOR_MONITOR_STARTUP
    ///</summary>
    [Description("申请交换指定车辆定位信息请求消息")]
    申请交换指定车辆定位信息请求消息 = 0x1207,
    ///<summary>
    ///取消交换指定车辆定位信息请求
    ///UP_EXG_MSG_APPLY_FOR_MONITOR_END
    ///</summary>
    [Description("取消交换指定车辆定位信息请求")]
    取消交换指定车辆定位信息请求 = 0x1208,
    ///<summary>
    ///补发车辆定位信息请求
    ///UP_EXG_MSG_APPLY_HISGNSSDATA_REQ
    ///</summary>
    [Description("补发车辆定位信息请求")]
    补发车辆定位信息请求 = 0x1209,
    ///<summary>
    ///上报车辆驾驶员身份识别信息应答
    ///UP_EXG_MSG_REPORT_DRIVER_INFO_ACK
    ///</summary>
    [Description("上报驾驶员身份识别信息应答消息")]
    上报驾驶员身份识别信息应答消息 = 0x120A,
    ///<summary>
    ///上报车辆电子运单应答	
    ///UP_EXG_MSG_TAKE_EWAYBILL_ACK
    ///</summary>
    [Description("上报车辆电子运单应答消息")]
    上报车辆电子运单应答消息 = 0x120B,
    ///<summary>
    ///主动上报驾驶员身份信息消息	
    ///UP_EXG_MSG_REPORT_DRIVER_INFO
    ///</summary>
    [Description("主动上报驾驶员身份信息消息")]
    主动上报驾驶员身份信息消息 = 0x120C,
    ///<summary>
    ///主动上报车辆电子运单信息	
    ///UP_EXG_MSG_REPORT_EWAYBILL_INFO
    ///</summary>
    [Description("主动上报车辆电子运单信息")]
    主动上报车辆电子运单信息 = 0x120D,
    ///<summary>
    ///主动上报车辆行驶路线信息	
    ///UP_BASE_MSG_DRVLINE_INFO
    ///</summary>
    [Description("主动上报车辆行驶路线信息")]
    主动上报车辆行驶路线信息 = 0x120E,


    #endregion

    #region 从链路动态信息交换消息 DOWN_EXG_MSG
    ///<summary>
    ///车辆注册信息应答消息	
    ///DOWN_EXG_MSG_REGISTER_ACK
    ///</summary>
    [Description("车辆注册信息应答消息")]
    车辆注册信息应答消息 = 0x9201,

    ///<summary>
    ///交换车辆定位信息	
    ///DOWN_EXG_MSG_CAR_LOCATION
    ///</summary>
    [Description("交换车辆定位信息消息")]
    交换车辆定位信息消息 = 0x9202,
    ///<summary>
    ///车辆定位信息交换补发	
    ///DOWN_EXG_MSG_HISTORY_ARCOSSAREA
    ///</summary>
    [Description("车辆定位信息交换补发消息")]
    车辆定位信息交换补发消息 = 0x9203,
    ///<summary>
    ///交换车辆静态信息
    ///DOWN_EXG_MSG_CAR_INFO
    ///</summary>
    [Description("交换车辆静态信息消息")]
    交换车辆静态信息消息 = 0x9204,
    ///<summary>
    ///启动车辆定位信息交换请求消息
    ///DOWN_EXG_MSG_RETURN_STARTUP
    ///</summary>
    [Description("启动车辆定位信息交换请求消息")]
    启动车辆定位信息交换请求消息 = 0x9205,
    ///<summary>
    ///结束车辆定位信息交换请求	
    ///</summary>
    [Description("结束车辆定位信息交换请求")]
    结束车辆定位信息交换请求 = 0x9206,
    ///<summary>
    ///申请交换指定车辆定位信息应答	
    ///DOWN_EXG_MSG_APPLY_FOR_MONITOR_STARTUP_ACK
    ///</summary>
    [Description("申请交换指定车辆定位信息应答")]
    申请交换指定车辆定位信息应答 = 0x9207,
    ///<summary>
    ///取消交换指定车辆定位信息应答or取消申请交换指定车辆定位信息应答消息	
    ///DOWN_EXG_MSG_APPLY_FOR_MONITOR_END_ACK
    ///</summary>
    [Description("取消交换指定车辆定位信息应答or取消申请交换指定车辆定位信息应答消息")]
    取消交换指定车辆定位信息应答or取消申请交换指定车辆定位信息应答消息 = 0x9208,
    ///<summary>
    ///补发车辆定位信息应答	
    ///DOWN_EXG_MSG_APPLY_HISGNSSDATA_ACK
    ///</summary>
    [Description("补发车辆定位信息应答")]
    补发车辆定位信息应答 = 0x9209,
    ///<summary>
    ///上报车辆驾驶员身份识别信息请求	
    ///DOWN_EXG_MSG_REPORT_DRIVER_INFO
    ///</summary>
    [Description("上报车辆驾驶员身份识别信息请求")]
    上报车辆驾驶员身份识别信息请求 = 0x920A,
    ///<summary>
    ///上报车辆电子运单请求	
    ///DOWN_EXG_MSG_TAKE_EWAYBILL_REQ
    ///</summary>
    [Description("上报车辆电子运单请求消息")]
    上报车辆电子运单请求消息 = 0x920B,
    ///<summary>
    ///上报车辆车辆行驶路线请求	
    ///DOWN_BASE_MSG_DRVLINE_REQ
    ///</summary>
    [Description("上报车辆车辆行驶路线请求")]
    上报车辆车辆行驶路线请求 = 0x920C,
    ///<summary>
    ///车辆行驶线路请求应答	
    ///DOWN_BASE_MSG_DRVLINE_ACK
    ///</summary>
    [Description("车辆行驶线路请求应答")]
    车辆行驶线路请求应答 = 0x920D,
    #endregion

    #region 主链路平台信息交互消息 UP_PLATFORM_MSG
    ///<summary>
    ///平台查岗应答消息	
    ///UP_PLATFORM_MSG_POST_QUERY_ACK
    ///</summary>
    [Description("平台查岗应答消息")]
    平台查岗应答消息 = 0x1301,
    ///<summary>
    ///下发平台间报文应答消息	
    ///UP_PLATFORM_MSG_INFO_ACK
    ///</summary>
    [Description("下发平台间报文应答消息")]
    下发平台间报文应答消息 = 0x1302,
    ///<summary>
    ///上传平台间消息补传请求消息	
    ///UP_PLATFORM_MSG_RETRAN_REQ
    ///</summary>
    [Description("上传平台间消息补传请求消息")]
    上传平台间消息补传请求消息 = 0x1303,

    #endregion

    #region 从链路平台信息交互消息 DOWN_PLATFORM_MSG
    ///<summary>
    ///平台查岗请求	
    ///DOWN_PLATFORM_MSG_POST_QUERY_REQ
    ///</summary>
    [Description("平台查岗请求")]
    平台查岗请求 = 0x9301,
    ///<summary>
    ///下发平台间报文请求	
    ///DOWN_PLATFORM_MSG_INFO_REQ
    ///</summary>
    [Description("下发平台间报文请求")]
    下发平台间报文请求 = 0x9302,
    ///<summary>
    ///下发平台间消息补传请求消息	
    ///DOWN_PLATFORM_MSG_RETRAN_REQ
    ///</summary>
    [Description("下发平台间消息补传请求消息")]
    下发平台间消息补传请求消息 = 0x9303,

    #endregion

    #region 主链路报警信息交互消息 UP_WARN_MSG
    ///<summary>
    ///报警督办应答消息	
    ///UP_WARN_MSG_URGE_TODO_ACK
    ///</summary>
    [Description("报警督办应答消息")]
    报警督办应答消息 = 0x1401,
    ///<summary>
    ///上报报警信息消息	
    ///UP_WARN_MSG_ADPT_INFO
    ///</summary>
    [Description("上报报警信息消息")]
    上报报警信息消息 = 0x1402,
    ///<summary>
    ///上报报警督办应答消息	
    ///UP_WARN_MSG_URGE_TODO_ACK_INFO
    ///</summary>
    [Description("上报报警督办应答消息")]
    上报报警督办应答消息 = 0x1411,
    ///<summary>
    ///主动上报报警处理结果消息	
    ///UP_WARN_MSG_ADPT_TODO_INFO
    ///</summary>
    [Description("主动上报报警处理结果消息")]
    主动上报报警处理结果消息 = 0x1412,
    ///<summary>
    ///上报报警督办请求消息	
    ///UP_WARN_MSG_URGE_TODO_REQ_INFO
    ///</summary>
    [Description("上报报警督办请求消息")]
    上报报警督办请求消息 = 0x1413,

    ///<summary>
    ///主动上报报警处理结果信息2013_上报报警预警信息2019	
    ///UP_WARN_MSG_ADPT_TODO_INFO_2013_UP_WARN_MSG_INFORM_TIPS_2019
    ///</summary>
    [Description("主动上报报警处理结果信息2013_上报报警预警信息2019")]
    主动上报报警处理结果信息2013_上报报警预警信息2019 = 0x1403,
    #endregion

    #region 从链路报警信息交互消息 DOWN_WARN_MSG
    ///<summary>
    ///报警督办请求消息	
    ///DOWN_WARN_MSG_URGE_TODO_REQ
    ///</summary>
    [Description("报警督办请求消息")]
    报警督办请求消息 = 0x9401,
    ///<summary>
    ///报警预警2013_下发报警预警消息2019	
    ///DOWN_WARN_MSG_INFORM_TIPS
    ///</summary>
    [Description("报警预警2013_下发报警预警消息2019")]
    报警预警2013_下发报警预警消息2019 = 0x9402,
    ///<summary>
    ///实时交换报警信息 	
    ///DOWN_WARN_MSG_EXG_INFORM
    ///</summary>
    [Description("实时交换报警信息")]
    实时交换报警信息 = 0x9403,
    #endregion

    #region 主链路车辆监管消息 UP_CTRL_MSG
    ///<summary>
    ///车辆单向监听应答	
    ///UP_CTRL_MSG_MONITOR_VEHICLE_ACK
    ///</summary>
    [Description("车辆单向监听应答")]
    车辆单向监听应答 = 0x1501,
    ///<summary>
    ///车辆拍照应答	
    ///UP_CTRL_MSG_TAKE_PHOTO_ACK
    ///</summary>
    [Description("车辆拍照应答")]
    车辆拍照应答 = 0x1502,
    ///<summary>
    ///下发车辆报文应答消息	
    ///UP_CTRL_MSG_TEXT_INFO_ACK
    ///</summary>
    [Description("下发车辆报文应答消息")]
    下发车辆报文应答消息 = 0x1503,
    ///<summary>
    ///上报车辆行驶记录应答消息	
    ///UP_CTRL_MSG_TAKE_TRAVEL_ACK
    ///</summary>
    [Description("上报车辆行驶记录应答消息")]
    上报车辆行驶记录应答消息 = 0x1504,
    ///<summary>
    ///车辆应急接入监管平台应答消息	
    ///UP_CTRL_MSG_EMERGENCY_MONITORING_ACK
    ///</summary>
    [Description("车辆应急接入监管平台应答消息")]
    车辆应急接入监管平台应答消息 = 0x1505,
    #endregion

    #region 从链路车辆监管消息 DOWN_CTRL_MSG
    ///<summary>
    ///车辆单向监听请求消息	
    ///DOWN_CTRL_MSG_MONITOR_VEHICLE_REQ
    ///</summary>
    [Description("车辆单向监听请求消息")]
    车辆单向监听请求消息 = 0x9501,
    ///<summary>
    ///车辆拍照请求消息	
    ///DOWN_CTRL_MSG_TAKE_PHOTO_REQ
    ///</summary>
    [Description("车辆拍照请求消息")]
    车辆拍照请求消息 = 0x9502,
    ///<summary>
    ///下发车辆报文请求	
    ///DOWN_CTRL_MSG_TEXT_INFO
    ///</summary>
    [Description("下发车辆报文请求")]
    下发车辆报文请求 = 0x9503,
    ///<summary>
    ///上报车辆行驶记录请求消息	
    ///DOWN_CTRL_MSG_TAKE_TRAVEL_REQ
    ///</summary>
    [Description("上报车辆行驶记录请求")]
    上报车辆行驶记录请求消息 = 0x9504,
    ///<summary>
    ///车辆应急接入监管平台请求消息	
    ///DOWN_CTRL_MSG_EMERGENCY_MONITORING_REQ
    ///</summary>
    [Description("车辆应急接入监管平台请求消息")]
    车辆应急接入监管平台请求消息 = 0x9505,
    #endregion

    #region 主链路静态信息交换消息 UP_BASE_MSG
    ///<summary>
    ///补报车辆静态信息应答	
    ///UP_BASE_MSG_VEHICLE_ADDED_ACK
    ///</summary>
    [Description("补报车辆静态信息应答")]
    补报车辆静态信息应答 = 0x1601,
    ///<summary>
    ///补报车辆行驶路线信息应答消息	
    ///UP_BASE_MSG_DRVLINE_ADDED_REQ
    ///</summary>
    [Description("补报车辆行驶路线信息应答消息")]
    补报车辆行驶路线信息应答消息 = 0x1602,

    #endregion

    #region 从链路静态信息交换消息 DOWN_BASE_MSG
    ///<summary>
    ///补报车辆静态信息请求消息
    ///DOWN_BASE_MSG_VEHICLE_ADDED
    ///</summary>
    [Description("补报车辆静态信息请求")]
    补报车辆静态信息请求消息 = 0x9601,
    #endregion
}