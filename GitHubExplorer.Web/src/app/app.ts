import { Component, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms'; 

export interface GitHubProjectResponseDto {
  gitHubProjectId: number;
  nameProject: string;
  description: string;
  starsCount: number;
  forksCount: number;
  watchersCount: number;
  ownerName: string;
  htmlUrl: string;
  createdAt: string;
  isFavoriteGitHubProject: boolean;
}
export enum RelevantOrderEnum {
  None = 0,
  Low = 1,
  Medium = 2,
  High = 3
}

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  standalone: false,
  styleUrl: './app.css'
})

export class App{

  protected readonly title = signal('GitHubExplorer');
  searchQuery: string = '';
  ownerQuery: string = '';
  projectQuery: string = '';
  repositories: GitHubProjectResponseDto[] = [];
  errorMessage: string = '';

  starsRelevantLevel: RelevantOrderEnum = RelevantOrderEnum.None;
  forksRelevantLevel: RelevantOrderEnum = RelevantOrderEnum.None;
  watchersRelevantLevel: RelevantOrderEnum = RelevantOrderEnum.None;

  relevantOptions = [
    { label: 'Nenhum', value: RelevantOrderEnum.None },
    { label: 'Baixo', value: RelevantOrderEnum.Low },
    { label: 'Médio', value: RelevantOrderEnum.Medium },
    { label: 'Alto', value: RelevantOrderEnum.High }
  ];

  constructor(private http: HttpClient) { }

  search(): void {
    this.errorMessage = '';

    let params = new URLSearchParams();

    if (this.ownerQuery) {
      params.append('OwnerName', this.ownerQuery);
    }
    if (this.projectQuery) {
      params.append('GitHubProjectName', this.projectQuery);
    }
    if (this.starsRelevantLevel !== RelevantOrderEnum.None) {
      params.append('StarsRelevantLevel', this.starsRelevantLevel.toString());
    }
    if (this.forksRelevantLevel !== RelevantOrderEnum.None) {
      params.append('ForksRelevantLevel', this.forksRelevantLevel.toString());
    }
    if (this.watchersRelevantLevel !== RelevantOrderEnum.None) {
      params.append('WatchersRelevantLevel', this.watchersRelevantLevel.toString());
    }

    const apiUrl = `https://localhost:7104/api/GitHubProject/GetFilterGitHubProjectAsync?${params.toString()}`;

    this.http.get<GitHubProjectResponseDto[]>(apiUrl).subscribe({
      next: (response) => {
        this.repositories = response;
        this.errorMessage = '';
      },
      error: (err) => {
        this.errorMessage = 'Erro ao buscar repositórios. Verifique a API ou a conexão.';
        this.repositories = [];
      }
    });
  }

  favorite(gitHubProjectId: number): void {
    const url = `https://localhost:7104/api/GitHubProject/FavoriteGitHubProject?gitHubProjectId=${gitHubProjectId}`;

    this.http.post<GitHubProjectResponseDto[]>(url, null).subscribe({
      next: (response) => {
        this.repositories = response;
        this.search();
      },
      error: (err) => {
        this.errorMessage = 'Erro ao favoritar o repositório.';
      }
    });
  }

}
