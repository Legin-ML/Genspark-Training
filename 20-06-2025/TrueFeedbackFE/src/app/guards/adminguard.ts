import { Injectable } from "@angular/core";
import { CanActivate, Router } from "@angular/router";
import { AuthService } from "../services/auth/auth.service";

@Injectable({providedIn: 'root'})
export class AdminGuard implements CanActivate{

    constructor(private auth: AuthService, private router: Router){
        
    }
    canActivate() : boolean{
        if (!this.auth.isAdmin()) {
            this.router.navigate(['/unauthorized'])
            return false;
        }
        return true;
    }

}