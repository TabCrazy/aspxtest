/**
 * @Name: 首页走势图
 * @Author: mikey.zhaopeng 
 * @Date: 2018-12-19 22:22:45 
 * @Last Modified by: mikey.zhaopeng
 * @Last Modified time: 2019-01-03 00:43:38
 */
$(function() {
    var _startDate = window.LOONYTAB.getFewDaysAgo(7);
    var _endDate = new Date().format('yyyy-MM-dd');
    var _typeid = null;
    // 获取到渲染chart的节点
    var trendChart = echarts.init(document.getElementById('chart-view'));
    // trendChart.setOption(window.LOONYTAB.echartsOpt);
    
    /**
     * 初始化日期组件
     */
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
        var tpl   =  $("#type-list-tpl").html();
        var template = Handlebars.compile(tpl);
        var html = template(params);
        $('#flower-type-idx').html(html);
        if (!window.LOONYTAB.isMobile) {
            $('#flower-type-idx').append('<li class="more">查看详情</li>')
        };
        $('#flower-type-idx').find('li').eq(0).addClass('curr')
        _typeid = data[0].id
        // 同时多个请求发起容易报错，修改成一个完成再发起下一个， 请求走势图数据
        
        window.LOONYTAB.loadRemarksData(renderRemark)
    };
    /**
     * 加载备注信息
     */
    function renderRemark(data) {
        // console.log(data)
        pageGetTrendData()
        var dom = ''
        for (var ox = 0 ; ox < data.length ; ox++) {
            dom += '<li>'+ data[ox].bz +'</li>'
        }
        $('#remarks-trend').find('ul').html(dom)
    };
    /**
     * 初始化获取趋势图数据
     */
    /**
     * @Name:请求数据
     * @param {*} param 
     */
    function pageGetTrendData() {
        var params = {
            startDate: _startDate,
            endDate: _endDate
        }
        params.isRecommend = true
        if (_typeid) {
            params.lx = _typeid
        } else {
            params.isRecommend = true
        }
        window.LOONYTAB.getTrendData(params, trendChart)
        
    }
    /**
     * 首页分类点击
     */
    addEvent()
    function addEvent() {
        // 时间区域
        $('#opt-btn-idx').find('.item').on("click", function(e){
            if ( $(this).is('.circbtn') ) {
                $(this).siblings().removeClass('curr')
                $(this).addClass('curr')
                handleClickCircbtn($(this).data('view-day'))
            }
        });
        // 类别点击查看详情
        $('#flower-type-idx').on("click", 'li', function(e){
            if ( $(this).is('.more') ) {
                // console.log('more')
                $(this).siblings().removeClass('curr')
                window.open(window.location.origin + '/products.aspx', "_self")
            } else {
                $(this).siblings().removeClass('curr')
                $(this).addClass('curr')
                // console.log($(this).data('type-id'))
                _typeid = $(this).data('type-id')
                pageGetTrendData()
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
        window.LOONYTAB.clearDateInput()
        _startDate = window.LOONYTAB.getFewDaysAgo(val);
        _endDate = new Date().format('yyyy-MM-dd');
        pageGetTrendData()
    }
    
})