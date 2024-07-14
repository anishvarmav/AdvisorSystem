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
  public advisor!: Advisor;
  public advisorForm!: FormGroup;
  public submitted: boolean = false;
  public isCreate: boolean = false;
  public preMaskedSin!: string;
  public healthStatuses: any[] = [{ label: 'Green', value: 'Green' }, { label: 'Yellow', value: 'Yellow' }, { label: 'Red', value: 'Red' }];

  constructor(private advisorService: AdvisorService, private messageService: MessageService, private formBuilder: FormBuilder) {
    this.advisorForm = this.formBuilder.group({
      id: [0],
      name: ['', Validators.required],
      sin: ['', [Validators.required, Validators.pattern('^\\d{9}$')]],
      address: [''],
      phone: ['', [Validators.pattern('^\\d{8}$')]],
      healthStatus: ['']
    });
  }

  ngOnInit() {
    this.advisorService.getAdvisors().subscribe({
      next: (data) => {
        this.advisors = data;
      }
    })
  }

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

  showCreateDialog(): void {
    this.resetAdvisorForm();
    this.submitted = false;
    this.isCreate = true;
    this.advisorSaveDialog = true;
  }

  showEditDialog(advisor: Advisor): void {
    this.resetAdvisorForm();
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

  hideSaveDialog(): void {
    this.resetAdvisorForm();
    this.preMaskedSin = '';
    this.submitted = false;
    this.advisorSaveDialog = false;
  }

  saveAdvisor(): void {
    this.submitted = true;
    if (!this.isCreate) {
      if (!this.advisorForm.get('sin')?.dirty) {
        this.advisorForm.get('sin')?.setValue(this.preMaskedSin);
      }
    }

    if (this.advisorForm.valid) {
      this.advisor = { ...this.advisorForm.value };
      if (this.isCreate) {
        this.advisorService.createAdvisor(this.advisor).subscribe({
          next: (result) => {
            if (result != null) {
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
        this.advisorService.updateAdvisorById(this.advisor.id, this.advisor).subscribe({
          next: (result) => {
            if (result != null) {
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

  showDeleteDialog(advisor: Advisor): void {
    this.advisor = { ...advisor };
    this.advisorDeleteDialog = true;
  }

  hideDeleteDialog(): void {
    this.advisorDeleteDialog = false;
  }

  deleteSelectedAdvisor(advisor: Advisor): void {
    this.advisorService.deleteAdvisorById(advisor.id).subscribe({
      next: (result) => {
        if (result) {
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
