/**
 * @Name: 内页鲜花走势图
 * @Author: mikey.zhaopeng 
 * @Date: 2018-12-19 22:21:59 
 * @Last Modified by: mikey.zhaopeng
 * @Last Modified time: 2019-01-03 23:33:15
 */
$(function() {
    var _startDate = window.LOONYTAB.getFewDaysAgo(7);
    var _endDate = new Date().format('yyyy-MM-dd');
    var _typeid = null; // 类型
    var _grade = null; // 级别
    var _classid = null; // 子类
    var insTrendChart = echarts.init(document.getElementById('chart-view-ins'));
    insTrendChart.setOption(window.LOONYTAB.echartsOpt);
    // 初始化日期组件
    window.LOONYTAB.initDate(setStartCallback, setEndCallback)
    function setStartCallback(instance, date) {
        // console.log(date)
        _startDate = new Date(date).format('yyyy-MM-dd');
    }
    function setEndCallback(instance, date) {
        // console.log(date)
        _endDate = new Date(date).format('yyyy-MM-dd');
    }
    /**
     * 渲染 type list 列表
     */
    window.LOONYTAB.getTypeData({}, renderTypeList)
    function renderTypeList(data) {
        var params = {list: data}
        if (!window.LOONYTAB.isMobile) {
            for(var iu = 0 ; iu < data.length ; iu++) {
                data[iu].lxmc = data[iu].lxmc + '行情报价'
            }
        }
        var tpl   =  $("#flower-trend-type-tpl").html();
        var template = Handlebars.compile(tpl);
        var html = template(params);
        $('#flower-trend-type').html(html);
        if ( window.location.search.substring(1).split("&")[0].split('=')[1] === "66" ) {
            _typeid = data[0].id
            $("#flower-trend-type").find('li').eq(0).addClass('curr')
        } else if ( window.location.search.substring(1).split("&")[0].split('=')[1] === "67" ) {
            _typeid = data[1].id
            $("#flower-trend-type").find('li').eq(1).addClass('curr')
        } else if ( window.location.search.substring(1).split("&")[0].split('=')[1] === "68" ) {
            _typeid = data[2].id
            $("#flower-trend-type").find('li').eq(2).addClass('curr')
        } else {
            _typeid = data[0].id
            $("#flower-trend-type").find('li').eq(0).addClass('curr')
        }
        // 需求多次修改，此处多次请求，暂不考虑这个性能消耗了
        // 同时多个请求发起容易报错，修改成一个完成再发起下一个， 请求鲜花等级数据
        window.LOONYTAB.getTypeData({id: _typeid}, renderLevelList)
    };
    /**
     * 渲染 等级 列表
     */
    function renderLevelList(data) {
        var params = {list: window.LOONYTAB.filterLevelOrSubclassData(data, '2')}
        var tpl   =  $("#flowe-level-tpl").html();
        var template = Handlebars.compile(tpl);
        var html = template(params);
        $('#flowe-level').html(html);
        // 需求多次修改，此处多次请求，暂不考虑这个性能消耗了
        // 同时多个请求发起容易报错，修改成一个完成再发起下一个， 请求鲜花子类 列表
        window.LOONYTAB.getTypeData({id: _typeid}, renderSubclassList)
    };
    /**
     * 渲染 子类 列表
     */
    function renderSubclassList(data) {
        var params = {list: window.LOONYTAB.filterLevelOrSubclassData(data, '1')}
        _classid = window.LOONYTAB.filterLevelOrSubclassData(data, '1')[0].id
        var tpl   =  $("#flowe-subclass-tpl").html();
        var template = Handlebars.compile(tpl);
        var html = template(params);
        $('#flowe-subclass').html(html);
        $('#flowe-subclass').find('li').eq(0).addClass('curr');
        // 需求多次修改，此处多次请求，暂不考虑这个性能消耗了
        // 同时多个请求发起容易报错，修改成一个完成再发起下一个， 获取走势图列表数据
        window.LOONYTAB.loadRemarksData(renderRemark)
    };
    /**
     * 加载备注信息
     */
    function renderRemark(data) {
        console.log(data)
        var dom = ''
        for (var ox = 0 ; ox < data.length ; ox++) {
            dom += '<li>'+ data[ox].bz +'</li>'
        }
        $('#remarks-trend').find('ul').html(dom)
        pageGetTrendData()
    }
    /**
     * @Name:请求数据
     * @param {*} param 
     */
    
    function pageGetTrendData(param) {
        var params = {
            startDate: _startDate,
            endDate: _endDate
        }
        if(_grade){
            params.jb = _grade
        }
        if (_classid) {
            params.zl = _classid
        }
        if(_typeid){
            params.lx = _typeid
        }
        if(!_typeid && !_grade && !_classid){
            params.isRecommend = true
        }
        window.LOONYTAB.getTrendData(params, insTrendChart)
    }
    /**
     * 点击分类
     */
    addEvent()
    function addEvent() {
        // 类别
        $('#flower-trend-type').on("click", 'li', function(e) {
            _grade = null
            _classid = null
            _typeid = $(this).data('typeid')
            $(this).siblings().removeClass('curr')
            $(this).addClass('curr')
            // pageGetTrendData()
            // 请求对应的子类数据
            window.LOONYTAB.getTypeData({id: _typeid}, renderLevelList)
        });
        // 等级
        $('#flowe-level').on("click", 'li', function(e){
            _grade = $(this).data('level')
            $(this).siblings().removeClass('curr')
            $(this).addClass('curr')
            pageGetTrendData()
        });
        // 子类
        $('#flowe-subclass').on("click", "li", function(e){
            _classid = $(this).data('subclass')
            $(this).siblings().removeClass('curr')
            $(this).addClass('curr')
            pageGetTrendData()
        });
        // 时间区域
        $('#opt-btn').find('.item').on("click", function(e){
            if ( $(this).is('.circbtn') ) {
                $(this).siblings().removeClass('curr')
                $(this).addClass('curr')
                handleClickCircbtn($(this).data('view-day'))
            }
        });
        // 自定义打开
        $("#cus-btn").on("click", function(e){
            $("#cus-date-dialog").show()
            // 完善确认的逻辑
            if (window.LOONYTAB.isMobile) {
                $("#date-mask").show()
            }
        });
        // 自定义确认
        $("#confirm").on("click", function(e){
            $('#opt-btn-idx').find('.item').removeClass('curr')
            $("#cus-date-dialog").hide()
            if (window.LOONYTAB.isMobile) {
                $("#date-mask").hide()
            }
            // 完善确认的逻辑
            pageGetTrendData()
        });
        // 移动端点击遮罩关闭时间选择框
        $("#date-mask").bind('click', function() {
            $("#cus-date-dialog").hide()
            $("#date-mask").hide()
        })
    }
    /**
     * 点击 7 or 30 天执行
     */
    function handleClickCircbtn(val) {
        window.LOONYTAB.clearDateInput();
        _startDate = window.LOONYTAB.getFewDaysAgo(val);
        _endDate = new Date().format('yyyy-MM-dd');
        pageGetTrendData();
    }
})