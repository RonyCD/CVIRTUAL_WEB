import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { PasswordModule } from 'primeng/password';
import { CardModule } from 'primeng/card';
import { TableModule } from 'primeng/table';
import { DropdownModule } from 'primeng/dropdown';
import { CalendarModule } from 'primeng/calendar';
import { DialogModule } from 'primeng/dialog';
import { TooltipModule } from 'primeng/tooltip';
import { ProgressBarModule } from 'primeng/progressbar';
import { CheckboxModule } from 'primeng/checkbox';

@NgModule({
  imports: [
    CommonModule
  ],
  exports: [
    ButtonModule,
    InputTextModule,
    PasswordModule,
    CardModule,
    TableModule,
    DropdownModule,
    CalendarModule,
    DialogModule,
    TooltipModule,
    ProgressBarModule,
    CheckboxModule
  ]
})
export class PrimengModule { }
