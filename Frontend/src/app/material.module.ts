import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";

import {MatToolbarModule} from '@angular/material/toolbar';
import {MatIconModule} from '@angular/material/icon';
import {MatButtonModule} from '@angular/material/button';
import {MatExpansionModule} from '@angular/material/expansion';
import {MatCardModule} from '@angular/material/card';
import {MatSidenavModule} from '@angular/material/sidenav';
import {MatListModule} from '@angular/material/list';
import {MatProgressSpinnerModule} from '@angular/material/progress-spinner';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatInputModule} from '@angular/material/input';
import {MatStepperModule} from '@angular/material/stepper';
import {MatSelectModule} from '@angular/material/select';
import {MatCheckboxModule} from '@angular/material/checkbox'; 
import {MatChipsModule} from '@angular/material/chips'; 
import {MatPaginatorModule} from '@angular/material/paginator';
import {MatMenuModule} from '@angular/material/menu'; 
import {MatDialogModule} from '@angular/material/dialog';
import {MatSnackBarModule} from '@angular/material/snack-bar';
import {MatDividerModule} from '@angular/material/divider'; 
import {MatGridListModule} from '@angular/material/grid-list';
import {MatDatepickerModule} from '@angular/material/datepicker'; 
import { MatNativeDateModule } from '@angular/material/core';
import {MatTooltipModule} from '@angular/material/tooltip';
import {MatAutocompleteModule} from '@angular/material/autocomplete'; 
import {CdkAccordionModule} from '@angular/cdk/accordion';
import {OverlayModule} from '@angular/cdk/overlay'; 
import {MatTreeModule} from '@angular/material/tree';
import {MatTableModule} from '@angular/material/table';
import {MatBadgeModule} from '@angular/material/badge';

@NgModule({
    declarations: [],
    imports: [
        CommonModule
    ],
    exports: [
      MatToolbarModule,
      MatIconModule,
      MatButtonModule,
      MatExpansionModule,
      MatCardModule,
      MatSidenavModule,
      MatListModule,
      MatProgressSpinnerModule,
      MatFormFieldModule,
      MatInputModule,
      MatStepperModule,
      MatSelectModule,
      MatCheckboxModule,
      MatChipsModule,
      MatPaginatorModule,
      MatMenuModule,
      MatDialogModule,
      MatSnackBarModule,
      MatDividerModule,
      MatDatepickerModule,
      MatGridListModule,
      MatNativeDateModule,
      MatTooltipModule,
      MatAutocompleteModule,
      CdkAccordionModule,
      OverlayModule,
      MatTreeModule,
      MatTableModule,
      MatBadgeModule
    ]
  })
  export class MaterialModule { }