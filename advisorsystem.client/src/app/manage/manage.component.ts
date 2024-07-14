import { Component } from '@angular/core';
import { AdvisorService } from '../advisor.service';
import { MessageService } from 'primeng/api';
import { Advisor } from '../advisor.model';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-manage',
  templateUrl: './manage.component.html',
  styleUrl: './manage.component.css'
})
export class ManageComponent {

  public advisorSaveDialog: boolean = false;
  public advisorDeleteDialog: boolean = false;
  public advisors!: Advisor[];
  // Current advisor being managed
  public advisor!: Advisor;
  // Reactive form for managing advisor data
  public advisorForm!: FormGroup;
  // Flag to track form submission state
  public submitted: boolean = false;
  // Flag to determine if creating a new advisor
  public isCreate: boolean = false;
  // Variable to store pre-masked SIN for updates
  public preMaskedSin!: string;
  public healthStatuses: any[] = [{ label: 'Green', value: 'Green' }, { label: 'Yellow', value: 'Yellow' }, { label: 'Red', value: 'Red' }];

  constructor(private advisorService: AdvisorService, private messageService: MessageService, private formBuilder: FormBuilder) {
    // Initialize the advisor form with validation rules
    this.advisorForm = this.formBuilder.group({
      id: [0],
      name: ['', Validators.required],
      sin: ['', [Validators.required, Validators.pattern('^\\d{9}$')]], // SIN must be 9 digits
      address: [''],
      phone: ['', [Validators.pattern('^\\d{8}$')]], // Phone must be 8 digits
      healthStatus: ['']
    });
  }

  // Fetch advisors on component initialization
  ngOnInit() {
    this.advisorService.getAdvisors().subscribe({
      next: (data) => {
        this.advisors = data;
      }
    })
  }

  // Returns a CSS class for severity based on the health status
  getHealthStatus(status: string) {
    switch (status) {
      case 'Green':
        return 'success';
      case 'Yellow':
        return 'warning';
      case 'Red':
        return 'danger';
      default:
        return 'success';
    }
  }

  // Opens the dialog for creating a new advisor
  showCreateDialog(): void {
    this.resetAdvisorForm();
    this.submitted = false;
    this.isCreate = true;
    this.advisorSaveDialog = true;
  }

  // Opens the dialog for editing an existing advisor
  showEditDialog(advisor: Advisor): void {
    this.resetAdvisorForm();
    // Store the original SIN for masking
    this.preMaskedSin = advisor.sin ?? '';

    this.advisorForm.setValue({
      id: advisor.id,
      name: advisor.name,
      sin: this.maskString(advisor.sin),
      address: advisor.address,
      phone: advisor.phone,
      healthStatus: advisor.healthStatus
    });

    this.submitted = false;
    this.isCreate = false;
    this.advisorSaveDialog = true;
  }

  // Hides the save dialog and resets the form
  hideSaveDialog(): void {
    this.resetAdvisorForm();
    this.preMaskedSin = '';
    this.submitted = false;
    this.advisorSaveDialog = false;
  }

  // Saves the advisor (either create or update)
  saveAdvisor(): void {
    this.submitted = true;
    if (!this.isCreate) {
      // If editing a current advisor, restore the original SIN if not modified
      if (!this.advisorForm.get('sin')?.dirty) {
        this.advisorForm.get('sin')?.setValue(this.preMaskedSin);
      }
    }

    if (this.advisorForm.valid) {
      this.advisor = { ...this.advisorForm.value };
      if (this.isCreate) {
        // Create a new advisor
        this.advisorService.createAdvisor(this.advisor).subscribe({
          next: (result) => {
            if (result != null) {
              // Update the advisors list
              this.advisors = [...this.advisors, result];
              this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'Advisor "' + this.advisor.name + '" created successfully.', life: 3000 });
              this.advisor = {};
            }
            else {
              this.messageService.add({ severity: 'error', summary: 'Failure', detail: 'Failed to create advisor "' + this.advisor.name + '". Please try again or contact support.', life: 3000 });
              this.advisor = {};
            }
          },
          error: () => {
            this.messageService.add({ severity: 'error', summary: 'Failure', detail: 'Failed to create advisor "' + this.advisor.name + '". Please try again or contact support.', life: 3000 });
            this.advisor = {};
          }
        })
      } else {
        // Update an existing advisor
        this.advisorService.updateAdvisorById(this.advisor.id, this.advisor).subscribe({
          next: (result) => {
            if (result != null) {
              // Update the advisors list
              this.advisors = this.advisors.map(val => (val.id === result.id ? result : val));
              this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'Advisor "' + this.advisor.name + '" updated successfully.', life: 3000 });
              this.advisor = {};
            }
            else {
              this.messageService.add({ severity: 'error', summary: 'Failure', detail: 'Failed to update advisor "' + this.advisor.name + '". Please try again or contact support.', life: 3000 });
              this.advisor = {};
            }
          },
          error: () => {
            this.messageService.add({ severity: 'error', summary: 'Failure', detail: 'Failed to update advisor "' + this.advisor.name + '". Please try again or contact support.', life: 3000 });
            this.advisor = {};
          }
        })
      }

      this.hideSaveDialog();
    }
  }

  // Opens the dialog for deleting an advisor
  showDeleteDialog(advisor: Advisor): void {
    this.advisor = { ...advisor };
    this.advisorDeleteDialog = true;
  }

  // Hides the delete dialog
  hideDeleteDialog(): void {
    this.advisorDeleteDialog = false;
  }

  // Deletes the selected advisor
  deleteSelectedAdvisor(advisor: Advisor): void {
    this.advisorService.deleteAdvisorById(advisor.id).subscribe({
      next: (result) => {
        if (result) {
          // Update the advisors list
          this.advisors = this.advisors.filter((val) => val.id !== advisor.id);
          this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'Advisor "' + advisor.name + '" deleted successfully.', life: 3000 });
          this.advisor = {};
        }
        else {
          this.messageService.add({ severity: 'error', summary: 'Failure', detail: 'Failed to delete advisor "' + advisor.name + '". Please try again or contact support.', life: 3000 });
          this.advisor = {};
        }
      },
      error: () => {
        this.messageService.add({ severity: 'error', summary: 'Failure', detail: 'Failed to delete advisor "' + advisor.name + '". Please try again or contact support.', life: 3000 });
        this.advisor = {};
      }
    })

    this.hideDeleteDialog();
  }

  // Resets the advisor form to its initial state
  resetAdvisorForm(): void {
    this.advisorForm.reset({
      id: 0,
      name: '',
      sin: '',
      address: '',
      phone: '',
      healthStatus: ''
    });
  }

  // Masks the given string to hide sensitive information with all but the last four characters
  maskString(value?: string): string {
    if (value != null) {
      if (value.length <= 4) {
        return value;
      } else {
        return '*'.repeat(value.length - 4) + value.slice(-4);
      }
    } else {
      return '';
    }
  }
}
