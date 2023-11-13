import { 
  ChangeDetectionStrategy, 
  Component,
  ElementRef, 
  EventEmitter, 
  HostListener, 
  Input, 
  OnInit, 
  Optional, 
  Output, 
  Self, 
  TemplateRef, 
  ViewChild
} from '@angular/core';
import { ControlValueAccessor, FormControl, NgControl } from '@angular/forms';
import { DropdownItem } from './store/dropdown-store.models';
import { MultiselectDropdownStore } from './store/dropdown.store';
import { Observable, debounceTime, distinctUntilChanged, skip } from 'rxjs';
import { BaseComponent } from '@core/components/base.component';

@Component({
  selector: "df-multiselect-dropdown",
  templateUrl: "./df-multiselect-dropdown.component.html",
  styleUrls: ["./df-multiselect-dropdown.component.scss"],
  changeDetection: ChangeDetectionStrategy.Default,
  providers: [MultiselectDropdownStore]
})
export class DfMultiSelectComponent<T extends DropdownItem> 
  extends BaseComponent implements ControlValueAccessor, OnInit  {
  private onChange: Function = (selectedItems: T[]) => {};

  private onTouched: Function = () => {};

  @Input('itemTemplate') 
  public itemTemplate: TemplateRef<any>;

  @Input('dropdownItems')
  public dropdownItems: Observable<T[]>;

  @Output('searchChanged')
  public searchChanged: EventEmitter<string> = new EventEmitter<string>();

  @Output('selectionOpened')
  public selectionOpened: EventEmitter<boolean> = new EventEmitter<boolean>();

  @ViewChild('searchInput') searchInput: ElementRef;

  public isDisabled: boolean = false;

  public searchControl: FormControl = new FormControl();

  constructor(
    @Self() @Optional() private control: NgControl,
    private elementRef: ElementRef, 
    public readonly store: MultiselectDropdownStore) {
    super();
    this.control.valueAccessor = this;
  }

  public get invalid(): boolean {
    return this.control ? this.control.invalid : false;
  }

  public get hasError(): boolean {
    const { touched, dirty } = this.control;
    return this.invalid ? (touched && dirty) : false;
  }

  public get touched(): boolean {
    return this.control.touched;
  }
  
  public ngOnInit(): void {
    this.dropdownItems.pipe(this.untilThis).subscribe((items) =>{
      this.store.updateItems(items);
    });
    this.searchControl.valueChanges.pipe(debounceTime(500), distinctUntilChanged(), this.untilThis)
      .subscribe((value) => {
        this.searchChanged.emit(value);
      });
    this.store.selectedItems$.pipe(skip(1), this.untilThis).subscribe(selectedItems => {
      this.onChange(selectedItems);
    });
    this.store.selectionOpened$.pipe(this.untilThis).subscribe(opened => {
      this.selectionOpened.emit(opened);
    });
  }

  @HostListener('document:mousedown', ['$event'])
  public onDocumentClick(event: any): void {
    if (!this.elementRef.nativeElement.contains(event.target)) {
      this.store.hideSelection();
    }
  }

  public onSearch(event: any): void {
    const inputElement = event.target as HTMLInputElement;
    this.adjustInputWidth(inputElement);
  }

  public onFocus(): void {
    if(!this.isDisabled) {
      this.store.openSelection();
      this.searchInput?.nativeElement.focus();
      this.onTouched();
    }
  }

  public onInputFocus(): void {
    if(!this.isDisabled) {
      this.store.openSelection();
      this.onTouched();
    }
  }

  public toggleSelection(): void {
    this.store.toggleSelection();
  }

  public selectItem(item: DropdownItem): void {
    this.searchControl.patchValue('');
    this.store.selectItem(item);
  }

  public removeSelected(item: DropdownItem): void {
    if(!this.isDisabled) {
      this.store.removeSelected(item);
    }
  }

  private adjustInputWidth(inputElement: HTMLInputElement) {
    const width = inputElement.value.length;
    const minWidth = '25px';
    if (width === 0 || width === 1) {
      inputElement.style.width = minWidth;
    } else if (inputElement.offsetWidth < inputElement.parentElement.offsetWidth) {
      inputElement.style.width = `${width + 3}ch`;
    }
  }

  public writeValue(selectedItems: T[]): void {
    if (selectedItems) {
      this.store.setSelectedItems(selectedItems);
    }
  }

  public registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  public registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  public setDisabledState(isDisabled: boolean): void {
    this.isDisabled = isDisabled;
    if(isDisabled) {
      this.searchControl.disable();
    } else {
      this.searchControl.enable();
    }
  }
}
