var io = require('socket.io')(6969)
var shortID = require('shortid')

var clients = []

io.on('connection', function (socket) {

	var currentUser

	socket.on('USER_CONNECT', function (data) {
		console.log('user has connect')
		// for (var i = 0; i < clients.length; i++) {
		// 	socket.emit('USER_CONNECTED', {
		// 		name:clients[i].name,
		// 		id:clients[i].id,
		// 		position:clients[i].position
		// 	})
		// 	console.log('User name: ' + clients[i].name + ' is connected..')
		// }
	})

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

	socket.on('disconnect', function (data) {
		socket.broadcast.emit('USER_DISCONNECTED', currentUser)
		for (var i = 0; i < clients.length; i++) {
			if (clients[i].name === currentUser.name && clients[i].id === currentUser.id){
				console.log('User: ' + clients[i].name + ' ID: ' + clients[i].id + ' has disconnected')
				clients.splice(i,1)
			}
		}
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
