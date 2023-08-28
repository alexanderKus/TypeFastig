#  Frontend TODO list

- [+] redirect to home page after successfull log in

- [+] user profile site

- [+] create dialog when typing is done

- [+] sending score to api

- [+] create top 100 ranking

- [ ] creating rooms  <- BIG

- [ ] write tests ???

- [+] properly align `Profile` button in navbar

- [ ] implement padding !!!

- [+] add some kind of spinner

-----------------------------
userProfiles: {
  username: '',
  scores: {
    withBestSpeed: {spped: 190, accuracy: 70},
    withBestAccuracy: {spped: 150, accuracy: 90}
  }
}

userHistory: {
  pageNumber: 0,
  pageSize: 10,
  data: [
    {spped: 0, accuracy: 0},
    {spped: 1, accuracy: 1},
    ...
  ]
}

AuthService {
  login();
  logout();
  register();
}

UserService {
  bestAccuarcy();
  bestSpeed();
  history();
  addScore();
}

ScoreSerivce {
  top100();
}

RoomService {
  joinRoom();
  leaveRoom();
}


