function SetPlayerInfo(div, interval = 1000, graph = false)
{    
    $.get("/api/cup", function (data) { 
        if (data) {
            if (graph) {                
                UpdateScore(data);
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

    setTimeout(function() { SetPlayerInfo(div, interval, graph) }, interval);  
}
var lastUsedData;
function UpdateScore(data)
{   
    lastUsedData = data;
    var players = [];
    for (key in data.players)
    {
        players.push({ Name: data.players[key].name, Wins: data.players[key].wins });
    }

    players.sort(function (x, y) {
        return x.Name > y.Name;
    });

    var gamesRequired = data.totalGames / 2.0;

    for (var i = 0; i < players.length; i++)
    {
        var $p = $($('.player')[i]).find('.progress');
        var val = players[i].Wins / gamesRequired * 100.0;
        $p.html('<div class="value" style="width: '+val+'%;"></div>');
    }
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