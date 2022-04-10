import { Component, OnInit, Output, EventEmitter, Inject } from '@angular/core';
import { HttpEventType, HttpClient } from '@angular/common/http';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-upload',
  templateUrl: './upload.component.html',
  styleUrls: ['./upload.component.css']
})
export class UploadComponent implements OnInit {
  public progress: number;
  public message: string;
  @Output() public onUploadFinished = new EventEmitter();
  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    public dialogRef: MatDialogRef<UploadComponent>,
    private http: HttpClient) { }
  ngOnInit() {
  }
  public uploadFile = (files) => {
    if (files.length === 0) {
      return;
    }
    let fileToUpload = <File>files[0];
    if (fileToUpload.type !== "application/pdf" && fileToUpload.type !== "image/jpeg") {
      this.message = 'File should be in pdf/png/jpeg format';
      return;
    }
    const formData = new FormData();
    formData.append('file', fileToUpload, fileToUpload.name);
    this.http.post('/api/file', formData, { reportProgress: true, observe: 'events' })
      .subscribe(event => {
        if (event.type === HttpEventType.UploadProgress)
          this.progress = Math.round(100 * event.loaded / event.total);
        else if (event.type === HttpEventType.Response) {
          this.message = 'Upload success.';
          this.onUploadFinished.emit(event.body);
        }
      });
  }
}
