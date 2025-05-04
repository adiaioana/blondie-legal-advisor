import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DocumentAnalyzerPageComponent } from './document-analyzer-page.component';

describe('DocumentAnalyzerPageComponent', () => {
  let component: DocumentAnalyzerPageComponent;
  let fixture: ComponentFixture<DocumentAnalyzerPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DocumentAnalyzerPageComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DocumentAnalyzerPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
