import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { Advisor } from "./advisor.model";

@Injectable({
    providedIn: 'root',
})
export class AdvisorService {
    constructor(private http: HttpClient) { }

    getAdvisors(): Observable<Advisor[]> {
        return this.http.get<Advisor[]>("http://localhost:5106/advisor");
    }

    getAdvisorById(id?: number): Observable<Advisor> {
        return this.http.get<Advisor>("http://localhost:5106/advisor/" + id);
    }

    createAdvisor(advisor?: Advisor): Observable<Advisor> {
        return this.http.put<Advisor>("http://localhost:5106/advisor", advisor);
    }

    updateAdvisorById(id?: number, advisor?: Advisor): Observable<Advisor> {
        return this.http.put<Advisor>("http://localhost:5106/advisor/" + id, advisor);
    }

    deleteAdvisorById(id?: number): Observable<boolean> {
        return this.http.delete<boolean>("http://localhost:5106/advisor/" + id);
    }
}