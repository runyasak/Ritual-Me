var io = require('socket.io')(6969)
var shortID = require('shortid')

var clients = []
// var count_player = 0 
var count_inGame = 0

io.on('connection', function (socket) {
	console.log('clients connected')
	var currentUser
	var timeToFight = 60
	socket.join('game room')

	socket.on('CHECK_DEVICE', function (data) {
		console.log("Device ID: " + data.device_id)
	})

	socket.on('USER_IN_GAME', function (data) {
		console.log(currentUser.name + ' is in game')
		count_inGame++
		console.log(count_inGame)
		socket.emit('PLAYER_ID', {id: currentUser.id})
		if(count_inGame == 2){
			io.to('game room').emit('START_GAME', {time: timeToFight})
		}
	})

	socket.on('USER_READY', function (data) {

		currentUser = {
			id: data.id,
			name: data.name
		}
		clients.push(currentUser)
		console.log(currentUser.name + " is ready")
		console.log("Number of clients: " + clients.length)

		if(clients.length == 2) {
			console.log('All users ready')
			io.to('game room').emit('CLICK_PLAY')
			count_timer()
		}
	})

	function count_timer () {
		setInterval(function() {
				if(timeToFight >= 0){
					console.log("Time to fight: " + timeToFight)
					io.to('game room').emit('FIGHT_TIMER', {time: timeToFight})
				    timeToFight--
				}
			}, 1000)
	}

	socket.on('SPAWN_WIZARD', function (data) {
		// console.log('spawn wizard!!')
		for (var i = 0; i < clients.length; i++) {
			if (clients[i].id === currentUser.id){
				console.log("Client: " + clients[i].name + " Spawn_wizard: " + data.rand)
			}
		}
		socket.broadcast.emit('SPAWN_WIZARD', data)
	})

	socket.on('START_FIGHT_PHASE', function (data) {
		for (var i = 0; i < clients.length; i++) {
			if (clients[i].id === currentUser.id){
				console.log("Client: " + clients[i].name + " has " + data.hp_wizard.length)
				for(var j = 0; j < data.hp_wizard.length; j++){
					console.log("Wizard " + j + " HP: " + data.hp_wizard[j]);
				}
			}
		}
		socket.broadcast.emit('START_FIGHT_PHASE', data)
	})

	socket.on('ATK_TO_PLAYER', function (data) {
		console.log('atk')
		for (var i = 0; i < clients.length; i++) {
			if (clients[i].id === currentUser.id){
				for (var i = 0; i < data.atk_arr.length; i++) {
					// console.log("Client: " + clients[i].name + "ATK " + i + " : " + data.atk_arr[i])
				}
			}
		}
		socket.broadcast.emit('ATK_TO_PLAYER', data)
	})

	socket.on('WIS_TO_PLAYER', function (data) {
		console.log('wis')
		for (var i = 0; i < clients.length; i++) {
			if (clients[i].id === currentUser.id){
				// console.log("Client: " + clients[i].name + "HEAL WIZARD: " + data.wizard_index + " with heal: " + data.heal_point)
			}
		}
		socket.broadcast.emit('WIS_TO_PLAYER', data)
	})

	socket.on('INT_TO_PLAYER', function (data) {
		console.log('int')
		for (var i = 0; i < clients.length; i++) {
			if (clients[i].id === currentUser.id){
				for (var i = 0; i < data.int_arr.length; i++) {
					// console.log("Client: " + clients[i].name + "INT " + i + " : " + data.int_arr[i])
				}
			}
		}
		socket.broadcast.emit('INT_TO_PLAYER', data)
	})


	socket.on('END_GAME', function (data) {
		console.log('GAME IS END!!')
		console.log(currentUser.name + " is lose")
		socket.broadcast.emit('END_GAME')
		console.log("count_inGame" + count_inGame)
	})


	socket.on('disconnect', function (data) {
		// socket.broadcast.emit('USER_DISCONNECTED', currentUser)
		for (var i = 0; i < clients.length; i++) {
			if (clients[i].id === currentUser.id){
				console.log('User ID: ' + clients[i].name + ' has disconnected')
				clients.splice(i,1)
				console.log("Number of clients: " + clients.length)
			}
		}
	})


})

console.log('------- server is running -------')
