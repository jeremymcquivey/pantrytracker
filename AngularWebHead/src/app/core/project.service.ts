import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Constants } from '../constants';
import { Project } from '../model/project';
import { Observable } from 'rxjs/Observable';
import { Milestone } from '../model/milestone';
import { UserPermission } from '../model/user-permission';
import { UserProfile } from '../model/user-profile';
import { MilestoneStatus } from '../model/milestone-status';
import { AuthService } from './auth.service';
import { Recipe } from '../model/recipe';

@Injectable()
export class ProjectService {
    constructor(private httpClient: HttpClient, private _authService: AuthService) { }
    
    getRecipes(): Observable<Recipe[]> {
        var accessToken = this._authService.getAccessToken();
        var headers = new HttpHeaders().set('Authorization', `Bearer ${accessToken}`);
        return this.httpClient.get<Recipe[]>(Constants.recipeApi + 'v1/Recipe', { headers: headers });
    }

    getRecipe(recipeId: string): Observable<Recipe> {
        var accessToken = this._authService.getAccessToken();
        var headers = new HttpHeaders().set('Authorization', `Bearer ${accessToken}`);
        return this.httpClient.get<Recipe>(Constants.recipeApi + 'v1/Recipe/' + recipeId, { headers: headers });
    }

    saveRecipe(recipe: Recipe) {
        var accessToken = this._authService.getAccessToken();
        var headers = new HttpHeaders().set('Authorization', `Bearer ${accessToken}`);

        if(recipe.id == "") {
            return this.httpClient.post<Recipe>(Constants.recipeApi + 'v1/Recipe/', { headers: headers });
        }
        return this.httpClient.put<Recipe>(Constants.recipeApi + 'v1/Recipe/' + recipe.id, recipe, { headers: headers });
    }
    

    getProjects(): Observable<Project[]> {
        var accessToken = this._authService.getAccessToken();
        var headers = new HttpHeaders().set('Authorization', `Bearer ${accessToken}`);
        return this.httpClient.get<Project[]>(Constants.apiRoot + 'Projects', { headers: headers });
    }

    getProject(projectId: number): Observable<Project> {
        return this.httpClient.get<Project>(Constants.apiRoot + 'Projects/' + projectId);
    }

    getProjectUsers(projectId: number): Observable<UserProfile[]> {
        return this.httpClient.get<UserProfile[]>(Constants.apiRoot + 'Projects/' + projectId + '/Users');
    }

    addProject(project: Project): Observable<Project> {
        return this.httpClient.post<Project>(Constants.apiRoot + 'Projects', project);
    }

    deleteProject(project: Project): Observable<object> {
        return this.httpClient.delete(Constants.apiRoot + 'Projects/' + project.id);
    }

    addUserPermission(userPermission: UserPermission) {
        return this.httpClient.post(Constants.apiRoot + 'UserPermissions', userPermission);
    }

    removeUserPermission(userId: string, projectId: number) {
        return this.httpClient.delete(`${Constants.apiRoot}UserPermissions/?userId=${userId}&projectId=${projectId}`);
    }

    updateUserPermission(userPermission) {
        return this.httpClient.put(`${Constants.apiRoot}UserPermissions`, userPermission);
    }

    getMilestones(projectId: number): Observable<Milestone[]> {
        return this.httpClient.get<Milestone[]>(Constants.apiRoot + 'Milestone');
    }

    getMilestoneStatuses() {
        return this.httpClient.get<MilestoneStatus[]>(`${Constants.apiRoot}Projects/MilestoneStatuses`);
    }

    addMilestone(milestone: Milestone) {
        return this.httpClient.post(`${Constants.apiRoot}Projects/Milestones`, milestone);
    }

    deleteMilestone(id: number) {
        return this.httpClient.delete(`${Constants.apiRoot}Projects/Milestones/${id}`);
    }

    updateMilestone(milestone: Milestone) {
        return this.httpClient.put(`${Constants.apiRoot}Projects/Milestones/${milestone.id}`, milestone);
    }
}