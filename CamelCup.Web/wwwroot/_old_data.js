function SetPlayerInfo(div, interval = 1000, graph = false, chart)
{    
    $.get("/api/cup", function (data) { 
        if (data) {
            if (graph) {
                if (!chart) {
                    chart = MakeChart(div);
                }
                
                UpdateGraph(chart, data);
            }
            else {
                var content = "<ul>";
                var players = data.players;
                players.sort(function (x, y) {
                    return x.wins < y.wins;
                });
            
                for (var key in players)
                {
                    var player = data.players[key]; 
                    content += "<li>" + player.name + ": " + player.wins+ "</li>";    
                }
            
                content += "</ul>";
                $(div).html(content); 
            }        
        }
    });

    setTimeout(function() { SetPlayerInfo(div, interval, graph, chart) }, interval);  
}

function MakeChart(div)
{
    var ctx = $(div)[0].getContext('2d');
    return new Chart(ctx, {
        type: 'horizontalBar',
        data: {
            labels: [],
            datasets: [{
                label: '# of Wins',
                data: [],
                backgroundColor: [
                    'rgba(255, 99, 132, 0.2)',
                    'rgba(54, 162, 235, 0.2)',
                    'rgba(255, 206, 86, 0.2)',
                    'rgba(75, 192, 192, 0.2)',
                    'rgba(153, 102, 255, 0.2)',
                    'rgba(255, 159, 64, 0.2)'
                ],
                borderColor: [
                    'rgba(255,99,132,1)',
                    'rgba(54, 162, 235, 1)',
                    'rgba(255, 206, 86, 1)',
                    'rgba(75, 192, 192, 1)',
                    'rgba(153, 102, 255, 1)',
                    'rgba(255, 159, 64, 1)'
                ],
                borderWidth: 1
            }]
        },
        options: {
            responsive: false,
            maintainAspectRatio: false,
            scales: {
                xAxes: [{
                    ticks: {
                        max : 1000,    
                        min : 0
                        //beginAtZero:true
                    }
                }]
            }
        }
    });
}

function UpdateGraph(chart, data)
{   
    labels = [];
    values = [];

    var players = data.players;
    players.sort(function (x, y) {
        return x.wins < y.wins;
    });

    for (var key in players)
    {
        var player = data.players[key];
        labels.push(player.name);
        values.push(player.wins);
    }

    chart.data.labels = labels;
    chart.data.datasets[0].data = values;

    chart.update();
}

function UploadForm(url, callback)
{
    var form = $(this).closest('form')[0];
    var fd = new FormData(form);
    $.ajax({
        url: url,
        type: 'POST',
        data: fd,
        cache: false,
        processData: false,
        contentType: false,
        success: function(data)
        {
            callback();
        }
    });
}

function HtmlEncode(value){
    return $('<div/>').text(value).html();
}