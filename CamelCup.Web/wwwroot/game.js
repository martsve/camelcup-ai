function CamelGame(div)
{
    self = this;
    self.Div = $(div);
    self.current = 0;
    
    self.Init = function() {
        var oldData = JSON.parse(window.localStorage.getItem('history'));
        if (oldData) 
            self.SetGameData(oldData);

        self.Reset();
    }

    self.SetGameData = function(data)
    {
        self.GameHistory = data;
    };

    self.UpdateGamedata = function(callback)
    {
        $.get("/api/cup", function (data) { 
            if (data.winner)
            {
                location.href = 'winner.html';
            }
        });       

        $.get("/api/cup/last", function (data) {
            self.SetGameData(data);
            window.localStorage.setItem('history', JSON.stringify(data));

            if (callback)
                callback(self);
        });
    };

    self.Iterate = function()
    {
        Next();
    };

    self.Reset = function() 
    {
        $div = self.Div;
        $div.empty();
        $div.append('<div id="boardRatio"></div>');
        $div.append('<ul class="players"></ul>');
        $div.append('<ul id="dice"></ul>');
        $div.append('<div id="locations"></div>');

        self.current = -1;

        var board = $('#locations');
        for (var i = 0; i < 16; i++)
        {
            board.append('<div class="loc" id="loc'+i+'"></div>');
        }

        $('#loc15').append('<div class="camel" id="camelBlue"></div>');
        $('#loc15').append('<div class="camel" id="camelGreen"></div>');
        $('#loc15').append('<div class="camel" id="camelWhite"></div>');
        $('#loc15').append('<div class="camel" id="camelRed"></div>');
        $('#loc15').append('<div class="camel" id="camelYellow"></div>');

        $('#history').html("<pre>" + JSON.stringify(self.GameHistory, null, 2) + "</pre>");

        ResetTraps();

        var players = [];
        for (key in self.GameHistory.players)
        {
            players.push({ Id: key, Name: self.GameHistory.players[key] });
        }

        players.sort(function (x, y) {
            return x.Name > y.Name;
        });

        for (key in players)
        {
            var name = HtmlEncode(players[key].Name);
            var id = players[key].Id;
            $div.find('.players').append('<li class="player" id="player'+id+'"><span class="name"></span> <span class="money"></span><div class="progress"></div><div class="winnerbets"></div><div class="loserbets"></div><div class="cards"></div><div class="trap" id="trap'+id+'"><span class="value"></span><span class="owner">'+name+'</span></div></li>');
            $('#player' + id + ' .name').html(name);
            UpdateMoney(id, 0);
        }

        if (lastUsedData)
            UpdateScore(lastUsedData);
    };

    self.GetLength = function()
    {
        return self.GameHistory.history.length
    };

    function UpdateMoney(player, money)
    {
        $('#player' + player + ' .money').html(money);
    }

    function ThrowDice(color, value) {
        $('#dice').append("<li><span class='card "+color+"'>"+value+"</span></li>");
    }

    function StartCamel(color, location, height)
    {
        var camel = $('#camel' + color).remove();
        camel.data('height', height)

        var smallest = null;
        $('#loc' + location + ' .camel').each(function() {
            var ch = $(this);
            if (ch.data('height') > height && (!smallest || ch.data('height') < smallest.data('height')))
                smallest = ch;
        });

        if (!smallest)
            $('#loc' + location).prepend(camel);
        else 
            smallest.before(camel);
    }

    function MoveStack(stack, from, to, top)
    {
        if (to > 15) to = to - 16;
        for (var i = 0; i < stack.length; i++)
        {
            var j = top ? i : (stack.length - i - 1);
            var color = stack[j];
            var camel = $('#camel' + color).remove();
            if (top)
                $('#loc' + to).prepend(camel);
            else 
                $('#loc' + to).append(camel);
        }
    }

    function PlaceTrap(player, location, move) {
        var trap = $('#trap' + player).remove();
        trap.removeClass('pluss');
        trap.removeClass('minus');
        trap.addClass(move > 0 ? 'pluss' : 'minus');
        trap.find('.value').html(move);
        $('#loc' + location).append(trap);
    }

    function ResetTraps() {
        for (player in self.GameHistory.players)
        {
            var trap = $('#trap' + player).remove();
            $('#player' + player).append(trap);
        }
    }

    function showWinner()
    {
        for (key in self.GameHistory.winners)
        {
            $("#player" + self.GameHistory.winners[key]).addClass("winner");
        }
    }

    function Next() 
    {
        self.current++;
        if (self.current >= self.GameHistory.history.length)
            return;

        var event = self.GameHistory.history[self.current];
        $('#current').html('<pre>'+self.current+ ': ' + JSON.stringify(event, null, 2) + '</pre>');
        if (event.action == "StartPosition")
        {
            StartCamel(event.color, event.value, event.height);
        } 
        else if (event.action == "GetMoney")
        {
            UpdateMoney(event.player, event.value);
        } 
        else if (event.action == "PickCard")
        {
            $('#player' + event.player + ' .cards').append('<span class="card '+event.color+'">'+event.value+'</span>');
        } 
        else if (event.action == "SecretBetOnWinner")
        {
            $('#player' + event.player + ' .winnerbets').append('<span class="card '+event.color+'">First</span>');
        } 
        else if (event.action == "SecretBetOnLoser")
        {
            $('#player' + event.player + ' .loserbets').append('<span class="card '+event.color+'">Last</span>');
        } 
        else if (event.action == "ThrowDice")
        {
            ThrowDice(event.color, event.value);
        } 
        else if (event.action == "Move")
        {
            MoveStack(event.stack, event.from, event.to, event.landOnTop);
        } 
        else if (event.action == "PlacePlussTrap")
        {
            PlaceTrap(event.player, event.value, 1);
        }
        else if (event.action == "PlaceMinusTrap")
        {
            PlaceTrap(event.player, event.value, -1);
        }
        else if (event.action == "NewRound")
        {
            $('#dice').empty();  
            $('.player .cards').empty(); 
            ResetTraps();
        }            
        else if (event.action == "Disqualified")
        {
            $('#player' + event.player).addClass("disqualified");
            $('#player' + event.player + ' .money').html(0);
        }

        if (self.current >= self.GameHistory.history.length -1) 
        {
            showWinner();   
        }
    }

    self.Init();
}
