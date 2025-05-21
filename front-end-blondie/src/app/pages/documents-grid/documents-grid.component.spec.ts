import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DocumentsGridComponent } from './documents-grid.component';

describe('DocumentsGridComponent', () => {
  let component: DocumentsGridComponent;
  let fixture: ComponentFixture<DocumentsGridComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DocumentsGridComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DocumentsGridComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
