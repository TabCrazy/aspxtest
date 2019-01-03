/**
 * @Name： 内页鲜花走势文字列表及详情
 * @Author: mikey.zhaopeng 
 * @Date: 2018-12-19 22:22:19 
 * @Last Modified by: mikey.zhaopeng
 * @Last Modified time: 2019-01-03 23:45:21
 */
$(function() {
    var _typeId = null;
    var _listData = null;
    var _nextData = null
    var _lastData = null
    /**
     * 渲染 type list 列表
     */
    window.LOONYTAB.getTypeData({}, renderTypeList)
    function renderTypeList(data) {
        _typeId = data[0].id
        if (!window.LOONYTAB.isMobile) {
            for(var iu = 0 ; iu < data.length ; iu++) {
                data[iu].lxmc = data[iu].lxmc + '行情报价'
            }
        }
        var params = {list: data}
        var tpl   =  $("#type-list-tpl").html();
        var template = Handlebars.compile(tpl);
        var html = template(params);
        $('#flower-type').html(html);
        if (!window.LOONYTAB.isMobile) {
            $('#flower-type').append('<li data-type-id="trend" class="more">查看走势图报价</li>')
        } else {
            $('#flower-type').append('<li data-type-id="trend" class="more">走势图</li>')
        }
        
        getData();
    };
    /**
     * 获取list数据
     */
    function getData(){
        var url = window.LOONYTAB.HOST + '?method=GetTrend&lx='+_typeId;
        $.get(url).done(function(res) {
            // 没有翻页，截取前48个元素
            _listData = JSON.parse(res).slice(0, 48)
            renderList(JSON.parse(res))
        })
    }
    /**
     * 获取详情数据
     */
    function getDataDetail(id){

        var url = window.LOONYTAB.HOST + '?method=GetFlowerPrice&lb='+_typeId+'&id='+id;
        $.get(url).done(function(res) {
            renderDetail(JSON.parse(res), id)
        })
    }
    /**
     * 渲染 list 列表
     */
    function renderList(data) {
        var params = {list: data}
        var tpl   =  $("#trend-list-tpl").html();
        var template = Handlebars.compile(tpl);
        var html = template(params);
        $('.trend-list').find('ul').html(html);
    };
    /**
     * 列表点击事件
     */
    $('#click-trend-list').on("click",'li', function(e){
        var id = $(this).data('trend-id');
        getDataDetail(id);
        nextLastData(id);
        $('#trend-detail').show()
        $('#trend-list').hide()
    });
    /**
     * 渲染 详情 列表
     */
    function renderDetail(data, id) {
        var params = {
            title: "**********",
            date: '****-**-**',
            detailList: data
        }
        for (var ix = 0; ix < _listData.length; ix++) { 
            if (_listData[ix].id.toString() === id.toString()) {
                params.title = _listData[ix].title
                params.date = _listData[ix].date
            }
        }
        var tpl   =  $("#trend-detail-tpl").html();
        var template = Handlebars.compile(tpl);
        var html = template(params);
        $('#trend-detail').html(html);
        // 渲染上一条下一条
        renderNextLast()
    };

    function renderNextLast() {
        if(_lastData) {
            $('#last-trend').html('<span>上一篇:<a>'+ _lastData.title +'</a></span>');
        }
        if(_nextData) {
            $("#next-trend").html('<span>下一篇:<a>'+ _nextData.title +'</a></span>')
        }
        addNextLastEvent()
    }
    function addNextLastEvent() {
        $('#last-trend').unbind('click')
        $('#next-trend').unbind('click')
        $('#last-trend').bind('click', 'a', function() {
            // console.log('last')
            getDataDetail(_lastData.id);
            nextLastData(_lastData.id);
        })
        $('#next-trend').bind('click', 'a', function() {
            // console.log('next')
            getDataDetail(_nextData.id);
            nextLastData(_nextData.id);
        })
    }

    /**
     * 上一条，下一条逻辑实现
     */
    function nextLastData(id) {
        for (var io = 0; io < _listData.length; io++) {
            if (id.toString() === _listData[io].id.toString()) {
                _nextData = _listData[io + 1]
                _lastData = _listData[io - 1]
            }
        }
        // console.log(_nextData, _lastData)
    }
    /**
     * 返回列表
     */
    $('body').on("click", '#back-list', function(e){
        // console.log('back')
        $('#trend-detail').hide()
        $('#trend-list').show()
    });
    /**
     * 点击分类
     */
    $('#flower-type').on("click", 'li', function(e){
        _typeId = $(this).data('type-id');
        if ( _typeId === 'trend') {
            window.open(window.location.origin + '/products.aspx', "_self")
        } else {
            $(this).siblings().removeClass('curr')
            $(this).addClass('curr')
            getData()
            $('#trend-detail').hide()
            $('#trend-list').show()
        }
    });
    
})