import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-server-not-found',
  templateUrl: './server-not-found.component.html',
  styleUrls: ['./server-not-found.component.css']
})
export class ServerNotFoundComponent implements OnInit {
 error:any;
  constructor(private router:Router) {
    const navigation=this.router.getCurrentNavigation();
    this.error=navigation?.extras?.state?.error;
    
   }

  ngOnInit(): void {
  }

}
