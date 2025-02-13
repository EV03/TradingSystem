import { Component, OnInit } from '@angular/core';
import { ReportService } from '../services/ReportService';
import { SupplierDeliveryTime } from '../models/SupplierDeliveryTime';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-report',
  templateUrl: './report.component.html',
  standalone: true,
  styleUrls: ['./report.component.css'],
  imports: [
    FormsModule,
    CommonModule
  ]
})
export class ReportComponent implements OnInit {
  reports: SupplierDeliveryTime[] = [];

  constructor(private reportService: ReportService) { }

  ngOnInit(): void { }

  loadReports(): void {{
      this.reportService.showAllReports().subscribe({
        next: (data: any) => {
          this.reports = data;
        },
        error: (error: any) => {
          console.error('Error fetching reports', error);
        }
      });
    }
  }
}
