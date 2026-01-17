import { Component, inject } from '@angular/core';
import { UsersService } from '../../services/users/users-service';
import { toSignal } from '@angular/core/rxjs-interop';

@Component({
  selector: 'app-users',
  imports: [],
  templateUrl: './users.html',
  styleUrl: './users.css',
})


export class Users {

  usersService: UsersService = inject(UsersService)
  usersResponse = toSignal(this.usersService.getUsers(), {initialValue: []})

  ngOnInit(){
   this.usersService.getUsers().subscribe(res => {console.log(res)})
  }
}
