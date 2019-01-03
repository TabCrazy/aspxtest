/**
 * @Name： 公共文件类
 * @Author: mikey.zhaopeng 
 * @Date: 2018-12-19 00:02:32 
 * @Last Modified by: mikey.zhaopeng
 * @Last Modified time: 2019-01-03 23:15:32
 */
window.LOONYTAB = {
}

window.LOONYTAB.isMobile = $(window).innerWidth() <= 750
$(window).resize(function(){
    window.LOONYTAB.isMobile = $(window).innerWidth() <= 750
});

// window.LOONYTAB.HOST = 'http://qiye.semsys.cn/handler.ashx'
// window.LOONYTAB.HOST = 'http://web1-53073.ns08.qiwang.cn/handler.ashx'
// window.LOONYTAB.HOST = 'http://101.201.221.29:28037/handler.ashx'
window.LOONYTAB.HOST = 'http://localhost:8088/handler.ashx'


/**
 * 初始化echarts配置
 */
var colors = ['#37A2DA', '#32C5E9', '#67E0E3', '#9FE6B8', '#FFDB5C', '#ff9f7f', '#fb7293', '#E062AE', '#E690D1', '#e7bcf3', '#9d96f5', '#8378EA', '#96BFFF']
window.LOONYTAB.echartsOpt = {
    title: {
        show: false
    },
    color: colors,
    tooltip : {
        trigger: 'axis',
        axisPointer: {
            type: 'cross',
            label: {
                backgroundColor: '#6a7985'
            }
        }
    },
    toolbox: {
        // 工具箱
        show: false
    },
    grid: {
        left: window.LOONYTAB.isMobile? '35' : '40',
        right: window.LOONYTAB.isMobile? '15' : '50',
        top: window.LOONYTAB.isMobile? '90' : '111',
        bottom: window.LOONYTAB.isMobile? '15' : null,
        containLabel: true
    },
    yAxis : [
        {
            type: 'value',
            name: '价格',
            position: 'right',
            // max: 'dataMax',
            max: function(value) {
                return value.max + 20;
            },
            axisLine: {
                lineStyle: {
                    color: '#909ea3',
                    width: 1,
                }
            },
            axisTick: {
                inside: true
            },
            axisLabel: {
                // 刻度值设置
                show: true,
                inside: true
            }
        }
    ],
    legend: {
    },
    xAxis : [
        {
            type : 'category',
            boundaryGap : false,
            data : []
        }
    ],
    series : []
};

window.LOONYTAB.getTypeData = function getTypeData(params, callback){
    var url = window.LOONYTAB.HOST + '?method=GetCategory'
    $.each(params, function(key, value) {
        url += '&' + key + '=' + value
    })
    $.get(url).done(function(res) {
        // console.log(res)
        callback(JSON.parse(res))
    })
}

/**
 * 过滤等级或子类数据
 */
window.LOONYTAB.filterLevelOrSubclassData = function filterLevelOrSubclassData(data, type) {
    var tempData = []
    $.each(data, function(idx, item) {
        if (item.zl === type) {
            tempData.push(item)
        }
    })
    return tempData
}

/**
 * 获取鲜花备注信息
 */

window.LOONYTAB.loadRemarksData = function loadRemarksData(callback) {
    $.get(window.LOONYTAB.HOST + '?method=GetRemarks').done(function(res) {
        // console.log(res)
        callback(JSON.parse(res))
    })
}

/**
 * 请求获取到数据后、将数据结构重组并且渲染到页面
 */
window.LOONYTAB.renderChartData = function renderChartData(data, charts) {
    let legendData = []
    let seriesData = []
    // 拿到请求得到的数据、循环构建echarts需要的数据结构
    for( var i = 0; i < data.trendData.length; i++ ) {
        legendData.push(data.trendData[i].name)
        seriesData.push({
            name: data.trendData[i].name,
            type:'line',
            // symbol:'none',
            smooth:true,
            stack: null,
            itemStyle: {
                opacity: 0
            },
            areaStyle: {
                opacity: 0.1
            },
            data: data.trendData[i].list
        })
    }
    // 组合数据结构
    let dynaOption = {
        legend: {
            type: 'scroll',
            left: window.LOONYTAB.isMobile? '30' : '36',
            top: window.LOONYTAB.isMobile? '10' : '30',
            right: window.LOONYTAB.isMobile? '10' : '280',
            data: legendData
        },
        xAxis : [
            {
                type : 'category',
                boundaryGap : false,
                data : data.dateArr
            }
        ],
        series: seriesData
    }
    charts.clear();
    charts.setOption(window.LOONYTAB.echartsOpt);
    charts.setOption(dynaOption);
    window.timer = null;
    $(window).resize(function(){
        if (window.timer) {
            clearTimeout(window.timer)
        }
        window.timer = setTimeout(function(){
            charts.resize()
        },300)
    });
}

/**
 * 获取趋势图数据公共请求方法
 */
window.LOONYTAB.getTrendData = function getTrendData(params, chart) {
    var url = window.LOONYTAB.HOST + '?method=GetTrend'
    $.each(params, function(key, value) {
        url += '&' + key + '=' + value
    })
    chart.showLoading()
    $.get(url).done(function(res){
        // console.log(res)
        res = JSON.parse(res)
        var tempData = {}
        var dateArr = []
        var trendData = []
        for(var i=0; i< res.length; i++) {
            dateArr.unshift(res[i].date)
            var templist = res[i].list
            for (var j=0; j<templist.length; j++) {
                trendData[j] = trendData[j] || {name: '', list: []}
                trendData[j].name = templist[j].mc
                trendData[j].list.unshift(templist[j].price)
            }
        }
        tempData.dateArr = dateArr
        tempData.trendData = trendData
        window.LOONYTAB.renderChartData(tempData, chart)
        chart.hideLoading();
        // console.log(tempData)
    })
}

/**
 * 获取今天的几天前的日期
 */

window.LOONYTAB.getFewDaysAgo = function getFewDaysAgo(num) {
    var millisecond = num * 24 * 60 * 60 * 1000;
    return new Date( new Date() - millisecond ).format('yyyy-MM-dd')
}


window.LOONYTAB.initDate = function initDate(startCallbak, endCallback) {
    /**
     * 日期组件
     */
    var startDate = datepicker('#start-date', {
        formatter: function(el, date, instance) {
            // console.log( new Date(date).format('yyyy-MM-dd'))
            el.value = new Date(date).format('yyyy-MM-dd');
        },
        customDays: ['S', 'M', 'T', 'W', 'Th', 'F', 'S'],
        customMonths: ['1', '2', '3', '4', '5', '6', '7', '8', '9', '10', '11', '12'],
        onSelect: function(instance, date) {
            startCallbak && startCallbak(instance, date)
        }
    })
    var endDate = datepicker('#end-date', {
        formatter: function(el, date, instance) {
            // console.log(date)
            el.value = new Date(date).format('yyyy-MM-dd');
        },
        customDays: ['S', 'M', 'T', 'W', 'Th', 'F', 'S'],
        customMonths: ['1', '2', '3', '4', '5', '6', '7', '8', '9', '10', '11', '12'],
        onSelect: function(instance, date) {
            endCallback && endCallback(instance, date)
        }
    })
}

window.LOONYTAB.clearDateInput = function clearDateInput() {
    $("#start-date").val('')
    $("#end-date").val('')
}

/**
 * ======================================================================================util function start ==============
 */
Date.prototype.format = function(fmt) {
    var o = {   
        "M+" : this.getMonth()+1,                 //月份   
        "d+" : this.getDate(),                    //日   
        "h+" : this.getHours(),                   //小时   
        "m+" : this.getMinutes(),                 //分   
        "s+" : this.getSeconds(),                 //秒   
        "q+" : Math.floor((this.getMonth()+3)/3), //季度   
        "S"  : this.getMilliseconds()             //毫秒   
    };   
    if(/(y+)/.test(fmt))
        fmt = fmt.replace(RegExp.$1, (this.getFullYear()+"").substr(4 - RegExp.$1.length));
    for(var k in o)
        if(new RegExp("("+ k +")").test(fmt)) 
    fmt = fmt.replace(RegExp.$1, (RegExp.$1.length==1) ? (o[k]) : (("00"+ o[k]).substr((""+ o[k]).length)));   
    return fmt;   
};