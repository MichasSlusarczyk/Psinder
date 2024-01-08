import {
  animate,
  state,
  style,
  transition,
  trigger,
} from '@angular/animations';
import { Component, OnInit } from '@angular/core';
import { User } from '../models/User';
import { UserService } from '../services/user.service';
import { EnumsService } from 'src/app/shared/services/enums/enums.service';
import { Router } from '@angular/router';
import { ShelterService } from '../../shelter/services/shelter.service';
import { Shelter } from '../../shelter/models/Shelter';

@Component({
  selector: 'app-users-page',
  templateUrl: './users-page.component.html',
  styleUrls: ['./users-page.component.scss'],
  animations: [
    trigger('detailExpand', [
      state('collapsed', style({ height: '0px', minHeight: '0' })),
      state('expanded', style({ height: '*' })),
      transition(
        'expanded <=> collapsed',
        animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')
      ),
    ]),
  ],
})
export class UsersPageComponent implements OnInit {
  constructor(private userService: UserService, 
    private enumsService: EnumsService, 
    private router: Router) { }

  users!: User[];
  columnsToDisplay = ['name', 'email', 'phoneNumber', 'role', 'action', 'edit'];
  columnsToDisplayWithExpand = [...this.columnsToDisplay, 'expand'];
  user!: User | null;
  shelters: Shelter[] = []

  ngOnInit(): void {
    this.getUsers()
  }

  getUsers() {
    this.userService.getUsers().subscribe((users) => {
      this.users = users.users.filter(user => !!user.accountStatus && user.accountStatus !== 4);
    });
  }

  blockUser(user: User) {
    this.userService.blockUser(user.id!).subscribe(() => {
      this.getUsers()
    });
  }

  editUser(user: User) {
    this.router.navigate([`/user-form/${user.id}`]);
  }

  getActionButton(user: User): string {
    return localStorage.getItem('language') === 'en' ? 'Block user' : 'Zablokuj użytkownika'
  }

  getRole(role: number) {
    return this.enumsService.getRoleFromOrdinal(role)
  }

  isUserBlocked(user: User) {
    return user?.accountStatus !== 2
  }
}
