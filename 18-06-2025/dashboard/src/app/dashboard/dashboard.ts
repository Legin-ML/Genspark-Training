import { Component, Input, OnInit } from '@angular/core';
import { UserModel } from '../models/usermodel';
import { Chart, registerables } from 'chart.js';
import * as L from 'leaflet';
import { UserService } from '../services/user.service';

Chart.register(...registerables);

@Component({
  selector: 'app-dashboard',
  imports: [],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css'
})
export class Dashboard implements OnInit {
  constructor(private userService: UserService) {}
   ngOnInit(): void {
    this.userService.getAllUsers().subscribe(users => {
    this.createPieChart(users);
    this.createBarChart(users);
    });
  }

  createPieChart(users : UserModel[]) {
    const maleCount = users.filter(u => u.gender.toLowerCase() === 'male').length;
    const femaleCount = users.filter(u => u.gender.toLowerCase() === 'female').length;

    new Chart('pieCanvas', {
      type: 'pie',
      data: {
        labels: ['Male', 'Female'],
        datasets: [{
          data: [maleCount, femaleCount],
          backgroundColor: ['#36A2EB', '#FF6384']
        }]
      }
    });
  }

  createBarChart(users:UserModel[]) {
    const roleCounts: { [role: string]: number } = {};
    for (const user of users) {
      const role = user.role || 'Unknown';
      roleCounts[role] = (roleCounts[role] || 0) + 1;
    }

    new Chart('barCanvas', {
      type: 'bar',
      data: {
        labels: Object.keys(roleCounts),
        datasets: [{
          label: 'User Roles',
          data: Object.values(roleCounts),
          backgroundColor: '#42a5f5'
        }]
      },
      options: {
        responsive: true,
        scales: {
          x: { beginAtZero: true },
          y: { beginAtZero: true }
        }
      }
    });
  }

  // initMap() {
  //   const map = L.map('map').setView([51.505, -0.09], 13);
  //   L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
  //     attribution: '© OpenStreetMap contributors'
  //   }).addTo(map);

  //   L.marker([51.505, -0.09]).addTo(map)
  //     .bindPopup('Sample Location')
  //     .openPopup();
  // }
}
