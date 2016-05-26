var io = require('socket.io')(6969)
var shortID = require('shortid')

var clients = []
// var count_player = 0

io.on('connection', function (socket) {
	console.log('clients connected')
	var currentUser
	socket.join('game room')

	socket.on('USER_CONNECT', function (data) {
		console.log('user has connect')
		currentUser = {
			id:shortID.generate()
		}
		clients.push(currentUser)
		// count_player++
		console.log("Number of clients: " + clients.length)

		for (var i = 0; i < clients.length; i++) {
			if (clients[i].id === currentUser.id){
				console.log("Client: " + clients[i].id)
			}
		}

		if(clients.length == 2){
			io.to('game room').emit('START_SPAWN_WIZARD')
		}
		// for (var i = 0; i < clients.length; i++) {
		// 	socket.emit('USER_CONNECTED', {
		// 		name:clients[i].name,
		// 		id:clients[i].id,
		// 		position:clients[i].position
		// 	})
		// 	console.log('User name: ' + clients[i].name + ' is connected..')
		// }
	})

	socket.on('START_GAME', function (data) {
		console.log('click play')
		io.to('game room').emit('CLICK_PLAY')
	})

	socket.on('SPAWN_WIZARD', function (data) {
		// console.log('spawn wizard!!')
		for (var i = 0; i < clients.length; i++) {
			if (clients[i].id === currentUser.id){
				console.log("Client: " + clients[i].id + " Spawn_wizard: " + data.rand)
			}
		}
		socket.broadcast.emit('SPAWN_WIZARD', data)
	})

	socket.on('START_FIGHT_PHASE', function (data) {
		for (var i = 0; i < clients.length; i++) {
			if (clients[i].id === currentUser.id){
				// console.log("Client: " + clients[i].id + " Spawn_wizard: " + data.rand)
				console.log("Client: " + clients[i].id + " has " + data.numb_wizard)
			}
		}
		socket.broadcast.emit('START_FIGHT_PHASE', data)
	})

	socket.on('ATK_TO_PLAYER', function (data) {
		console.log('atk')
		for (var i = 0; i < clients.length; i++) {
			if (clients[i].id === currentUser.id){
				// console.log("Client: " + clients[i].id + " Spawn_wizard: " + data.rand)
				// console.log("Client: " + clients[i].id + " has " + data.numb_wizard)
				for (var i = 0; i < data.atk_arr.length; i++) {
					console.log("Client: " + clients[i].id + "ATK " + i + " : " + data.atk_arr[i])
				}
			}
		}
		socket.broadcast.emit('ATK_TO_PLAYER', data)
	})

	socket.on('WIS_TO_PLAYER', function (data) {
		console.log('wis')
		for (var i = 0; i < clients.length; i++) {
			if (clients[i].id === currentUser.id){
				console.log("Client: " + clients[i].id + "HEAL WIZARD: " + data.wizard_index + " with heal: " + data.heal)
			}
		}
		socket.broadcast.emit('WIS_TO_PLAYER', data)
	})

	socket.on('disconnect', function (data) {
		// socket.broadcast.emit('USER_DISCONNECTED', currentUser)
		for (var i = 0; i < clients.length; i++) {
			if (clients[i].id === currentUser.id){
				console.log('User ID: ' + clients[i].id + ' has disconnected')
				clients.splice(i,1)
				console.log("Number of clients: " + clients.length)
			}
		}
	})

	//P'Wach code
	socket.on('PLAY', function (data) {

		currentUser = {
			name:data.name,
			id:shortID.generate(),
			position:data.position
		}
		console.log(currentUser.name + ' ' + currentUser.id + ' ' + currentUser.position)
		clients.push(currentUser)
		socket.emit('PLAY', currentUser)

		socket.broadcast.emit('USER_CONNECTED', currentUser)

	})

	socket.on('MOVE', function (data){

		// currentUser.name = data.name
		// currentUser.id = data.id
		currentUser.position = data.position
		socket.broadcast.emit('MOVE', currentUser)
		console.log(currentUser.name + ' Move to ' + currentUser.position)

	})

})

console.log('------- server is running -------')
