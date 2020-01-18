import { Component, OnInit } from "@angular/core";
import { MatTableDataSource } from "@angular/material";
import { ProjectService } from "../core/project.service";
import { Utils } from "../core/utils";
import { Project } from "../model/project";
import { Recipe } from "../model/recipe";

@Component({
  selector: "app-projects",
  templateUrl: "project-list.component.html"
})
export class ProjectListComponent implements OnInit {
  displayedColumns = ["name"];
  error: string;
  dataSource = new MatTableDataSource();
  dataSource2 = new MatTableDataSource();
  projects: Project[];
  recipes: Recipe[];

  constructor(private _projectService: ProjectService) {}

  ngOnInit() {
    this._projectService.getRecipes().subscribe(recipes => {
      this.recipes = recipes;
      this.dataSource2.data = recipes;
    }, error => Utils.formatError(error));

    /*this._projectService.getProjects().subscribe(projects => {
      this.projects = projects;
      this.dataSource.data = projects;
    }, error => Utils.formatError(error));*/
  }
}
