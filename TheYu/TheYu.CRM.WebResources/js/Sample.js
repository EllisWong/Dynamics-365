if (TheYu === undefined) { TheYu = {}; }

if (TheYu.CRM == undefined) { TheYu.CRM = {}; }

TheYu.CRM.SampleCode = (function () {
    'use strict';
    return {
        Constants: {
            Fields:
                {},

        },
        onLoad: function (executionContext) {

        },
        /**
         * SubGrid 设置查询条件
         * @param {any} formContext
         */
        filterSubgrid: function (formContext) {

            var filterConditionXml = '<filter type="and">'
                += '<condition attribute="{YOUR_FIELD}" operater="eq" value="{YOUR_CONDITION_VALUE}"/>'
                += '</filter>'
            var subgridCtrl = formContext.getControl("{YOUR_SUBGRID_CONTROL}");
            subgridCtrl.setFilterXml(subgridCtrl);
            subgridCtrl.refresh();
        },
        /**
         * 表单上的SubGrid显示方式为Chart的时候，
         * 给SubGrid设置Filter条件后图表无法刷新
         * @param {any} formContext
         */
        filterChart: function (formContext) {
            //1.设置SubGrid过滤条件
            var filterConditionXml = '<filter type="and">'
                += '<condition attribute="{YOUR_FIELD}" operater="eq" value="{YOUR_CONDITION_VALUE}"/>'
                += '</filter>'
            var subgridCtrl = formContext.getControl("{YOUR_SUBGRID_CONTROL}");
            subgridCtrl.setFilterXml(subgridCtrl);

            //2.获取当前图表使用的View
            var currentView = formContext.getControl("{YOUR_SUBGRID_CONTROL}").getViewSelector().getCurrentView();
            //通过当前视图ID获取视图的FetchXMl，重新构建一个FetchXML去替换或在原有的视图FetchXML追加条件。
            var newFetchXML = "<fetch></fetch>";
            formContext.getControl("{YOUR_SUBGRID_CONTROL}")._clientApiExecutorControl._store.getState().metadata.views[currentView.id].fetchXML = newFetchXML;
            //最后调用Control的刷新方法
            subgridCtrl.refresh();


        },
        refreshAllIframe: function (formContext) {

            var allControls = formContext.getControl().filter(x => x.name.indexOf("WebResource_") > -1);
            allControls.forEach(function (item) {
                try {
                    item.getObject().contentWidow.location.reload();
                } catch (e) {

                }
            });
        },
        replaceUrlParam: function (url, key, value) {
            if (url.indexOf(key) != -1) {
                var re = eval('/(' + key + '=)([^&]*)/gi');
                var newUrl = url.replace(re, key + '=' + value);
                return newUrl;


            }
            else {
                var newUrl = url + '&' + key + '=' + value;
                return newUrl;
            }
        }
    }

});