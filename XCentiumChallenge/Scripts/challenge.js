(function ($)
{
    xc = {
        vars: {
            $url: {},
            $results: {},
            $error: {},
            $submit: {},
            $chart: {}
        },
        init: function ()
        {
            var o = xc,
                v = o.vars;

            v.$url = $("#siteURL");
            v.$results = $("#results");
            v.$error = $("#error");
            v.$submit = $("#challengeSubmit");
            v.$chart = $("#charts");

            v.$results.hide();
            v.$error.hide();

            $("#challengeSubmit").on("click", o.SubmitUrl);
        },
        SubmitUrl: function ()
        {
            var o = xc,
               v = o.vars;

            v.$results.hide();
            v.$chart.html("");

            v.$error.hide();
            var url = $.trim(v.$url.val());
            if (url.length === 0)
            {
                v.$error.html("URL Is Required");
                v.$error.show();
                return false;
            }
            if (!o.IsValidURL(url))
            {
                v.$error.html("Invalid URL");
                v.$error.show();
                return false;
            }
            jQuery.ajax({
                type: 'Get',
                async: true,
                url: '/home/GetResults',
                data: {
                    siteURL: url
                },
                success: function (data)
                {
                    if (data.Success)
                    {
                        var source = document.getElementById("results-template").innerHTML;
                        var template = Handlebars.compile(source);
                        v.$results.html(template(data));
                        v.$results.show();

                        o.ShowCarousel(data);
                        o.ShowCharts(data);
                    } else
                    {
                        v.$results.hide();
                        v.$error.html(data.ErrorMessage);
                        v.$error.show();
                    }
                },
                error: function (xhr, textStatus, error)
                {
                    console.log(xhr.statusText);
                    console.log(textStatus);
                    console.log(error);
                    v.$error.html("Invalid Call");
                    v.$error.show();
                }
            });
            return false;
        },
        ShowCharts: function (data)
        {
            var o = xc,
              v = o.vars;

            var labels = [];
            var seriesData = [];

            for (var i = 0; i < data.Words.length; i++ ) {
                labels.push(data.Words[i].Word);

                var seriesDataPt = {};
                seriesDataPt.name = data.Words[i].Word;
                seriesDataPt.y = data.Words[i].Count;
                seriesData.push(seriesDataPt);
            }

            seriesData.data = seriesData;

            $("#chart").highcharts({
                chart: {
                    type: 'column'
                },

                xAxis: {
                    categories: labels
                },

                title: {
                    text: 'Word Count'
                },


                plotOptions: {
                    pie: {
                        allowPointSelect: true,
                        cursor: 'pointer',
                        dataLabels: {
                            enabled: true,
                            format: '<b>{point.name}</b>: {point.percentage:.d}',
                            style: {
                                color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                            }
                        }
                    }
                },
                series: [
                {
                    showInLegend: false,
                    data: seriesData
                }]
            });
        },
        ShowCarousel:function(data) {
            for (var i = 0 ; i < data.ImageUrl.length ; i++)
            {
                $('<div class="carousel-item"><img class="d-block mx-auto" src="' + data.ImageUrl[i] + '"></div>').appendTo('#challenge-carousel .carousel-inner');
            }
            $('#challenge-carousel .carousel-item').first().addClass('active');
            $('#carousel-example-generic').carousel();
        },
        IsValidURL: function (str)
        {
            var pattern = /(http|https):\/\/(\w+:{0,1}\w*)?(\S+)(:[0-9]+)?(\/|\/([\w#!:.?+=&%!\-\/]))?/;
            return pattern.test(str);
        }

    };

    var xc;
    xc.init();

})(jQuery);