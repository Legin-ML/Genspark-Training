import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, GuardResult, MaybeAsync, Router, RouterStateSnapshot } from "@angular/router";
import { AuthService } from "../services/auth/auth.service";
import { MessageService } from "primeng/api";

@Injectable({providedIn: "root"})
export class AuthGuard implements CanActivate{
    constructor(private auth: AuthService, private router: Router, private messageService : MessageService){
        
    }
    canActivate() : boolean{
        if (!this.auth.isLoggedIn()) {
            this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Unauthorized, Please login', life: 3000 })
            this.router.navigate(['/login'])
            return false;
        }
        return true
    }
}